using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.TransactionRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.TransactionServices;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;

    public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }
    public IList<GetTransactionDetail> GetTransactionPage(PagingParam<TransactionEnum.TransactionSort> paginationModel, SearchTransactionModel searchTransactionModel)
    {
        IQueryable<Transaction> queryTransaction = _transactionRepository.Table.Include(t => t.Wallet);
        queryTransaction = queryTransaction.GetWithSearch(searchTransactionModel);
        // Apply sort
        queryTransaction = queryTransaction.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryTransaction = queryTransaction.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetTransactionDetail>(queryTransaction);
        return result.ToList();
    }

    public async Task<GetTransactionDetail> GetTransactionById(Guid id)
    {
        Transaction workingStyle = await _transactionRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (workingStyle == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetTransactionDetail>(workingStyle);
        return result;
    }

    public async Task<GetTransactionDetail> CreateTransactionAsync(CreateTransactionModel requestBody)
    {
        Transaction workingStyle = _mapper.Map<Transaction>(requestBody);
        if (workingStyle == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        await _transactionRepository.InsertAsync(workingStyle);
        await _transactionRepository.SaveChangesAsync();
        GetTransactionDetail workingStyleDetail = _mapper.Map<GetTransactionDetail>(workingStyle);
        return workingStyleDetail;
    }

    public async Task<GetTransactionDetail> UpdateTransactionAsync(Guid id, UpdateTransactionModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        Transaction workingStyle = await _transactionRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (workingStyle == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        workingStyle = _mapper.Map(requestBody, workingStyle);
        _transactionRepository.Update(workingStyle);
        await _transactionRepository.SaveChangesAsync();
        GetTransactionDetail workingStyleDetail = _mapper.Map<GetTransactionDetail>(workingStyle);
        return workingStyleDetail;
    }

    public async Task DeleteTransactionAsync(Guid id)
    {
        Transaction workingStyle = await _transactionRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (workingStyle == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        _transactionRepository.Delete(workingStyle);
        await _transactionRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _transactionRepository.GetAll().CountAsync();
    }
}