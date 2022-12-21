using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.ApplicantRepositories;
using ITJob.Entity.Repositories.ProductRepositories;
using ITJob.Entity.Repositories.SystemWalletRepositories;
using ITJob.Entity.Repositories.TransactionRepositories;
using ITJob.Entity.Repositories.WalletRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.SystemWallet;
using ITJob.Services.ViewModels.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.TransactionServices;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly ISystemWalletRepository _systemWalletRepository;
    private readonly IProductRepository _productRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IApplicantRepository _applicantRepository;

    public TransactionService(ITransactionRepository transactionRepository, IMapper mapper, ISystemWalletRepository systemWalletRepository, IProductRepository productRepository, IWalletRepository walletRepository, IApplicantRepository applicantRepository)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _systemWalletRepository = systemWalletRepository;
        _productRepository = productRepository;
        _walletRepository = walletRepository;
        _applicantRepository = applicantRepository;
    }
    public IList<GetTransactionDetail> GetTransactionPage(PagingParam<TransactionEnum.TransactionSort> paginationModel,
        SearchTransactionModel searchTransactionModel)
    {
        IQueryable<Transaction> queryTransaction = _transactionRepository.Get()
            .Include(t => t.Product);
        // queryTransaction = queryTransaction.GetWithSearch(searchTransactionModel);
        if (searchTransactionModel.FromDate != null && searchTransactionModel.ToDate != null)
        {
            queryTransaction = queryTransaction.Where(t => t.CreateDate >= searchTransactionModel.FromDate
                                        && t.CreateDate <= searchTransactionModel.ToDate);  
        }
        if (searchTransactionModel.CreateBy != null)
        {
            queryTransaction = queryTransaction.Where(t => t.CreateBy == searchTransactionModel.CreateBy);
        }
        if (searchTransactionModel.TypeOfTransaction != null)
        {
            queryTransaction = queryTransaction.Where(t => t.TypeOfTransaction.Contains(searchTransactionModel.TypeOfTransaction));
        }
        if (searchTransactionModel.WalletId != null)
        {
            queryTransaction = queryTransaction.Where(t => t.WalletId == searchTransactionModel.WalletId);
        }
        // Apply sort
        queryTransaction = queryTransaction.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryTransaction = queryTransaction.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetTransactionDetail>(queryTransaction);
        return result.ToList();
    }

    public async Task<GetSystemWalletDetail> GetSystemWallet()
    {
        var querySystemWallet = _systemWalletRepository.GetAll();
        var result = _mapper.ProjectTo<GetSystemWalletDetail>(querySystemWallet);
        return result.FirstOrDefault();
    }
        
    public async Task<GetTransactionDetail> GetTransactionById(Guid id)
    {
        Transaction transaction = await _transactionRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (transaction == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetTransactionDetail>(transaction);
        return result;
    }

    public async Task<GetTransactionDetail> CreateTransactionAsync(CreateTransactionModel requestBody)
    {
        Transaction transaction = _mapper.Map<Transaction>(requestBody);
        if (transaction == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var product = await _productRepository.GetFirstOrDefaultAsync(e => e.Id == requestBody.ProductId);
        var total = product.Price * requestBody.Quantity;
        if(product.Quantity < requestBody.Quantity)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Quantity is not enough!!! ");
        }
        var applicant = await _applicantRepository.GetFirstOrDefaultAsync(e => e.Id == requestBody.CreateBy);
        var wallet = await _walletRepository.GetFirstOrDefaultAsync(w => w.ApplicantId == applicant.Id);
        if(wallet.Balance < total)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Balance is not enough!!! ");
        }
        //update wallet
        wallet.Balance -= total;
        _walletRepository.Update(wallet);
        await _walletRepository.SaveChangesAsync();
        //update product
        product.Quantity -= requestBody.Quantity;
        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync();
        //update system wallet
        var systemWalletId = new Guid("f4e64438-0fdb-44f7-8719-c69f1ac4ab67");
        var systemWallet = await _systemWalletRepository.GetFirstOrDefaultAsync(w => w.Id == systemWalletId);
        systemWallet.TotalOfSystem += total;
        _systemWalletRepository.Update(systemWallet);
        await _systemWalletRepository.SaveChangesAsync();
        //create transaction
        transaction.Total = total;
        transaction.TypeOfTransaction = "Reward exchange";
        transaction.WalletId = wallet.Id;
        transaction.ProductId = product.Id;
        transaction.Quantity = requestBody.Quantity;
        await _transactionRepository.InsertAsync(transaction);
        await _transactionRepository.SaveChangesAsync();
        
        GetTransactionDetail transactionDetail = _mapper.Map<GetTransactionDetail>(transaction);
        return transactionDetail;
    }

    public async Task<GetTransactionDetail> UpdateTransactionAsync(Guid id, UpdateTransactionModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        Transaction transaction = await _transactionRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (transaction == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        transaction = _mapper.Map(requestBody, transaction);
        _transactionRepository.Update(transaction);
        await _transactionRepository.SaveChangesAsync();
        GetTransactionDetail transactionDetail = _mapper.Map<GetTransactionDetail>(transaction);
        return transactionDetail;
    }

    public async Task DeleteTransactionAsync(Guid id)
    {
        Transaction transaction = await _transactionRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (transaction == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _transactionRepository.Delete(transaction);
        await _transactionRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _transactionRepository.GetAll().CountAsync();
    }
}