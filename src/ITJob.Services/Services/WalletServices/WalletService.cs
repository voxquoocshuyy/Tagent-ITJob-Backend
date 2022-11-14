using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.WalletRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Wallet;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ITJob.Services.Services.WalletServices;

public class WalletService : IWalletService
{
    private readonly double _exchangeRate;
    private readonly IWalletRepository _walletRepository;
    private readonly IMapper _mapper;

    public WalletService(IWalletRepository walletRepository, IMapper mapper, IConfiguration config)
    {
        _walletRepository = walletRepository;
        _mapper = mapper;
        _exchangeRate = double.Parse(config["SystemConfiguration:ExchangeRate"]);
    }
    public IList<GetWalletDetail> GetWalletPage(PagingParam<WalletEnum.WalletSort> paginationModel, SearchWalletModel searchWalletModel)
    {
        IQueryable<Wallet> queryWallet = _walletRepository.Table.Include(c => c.Transactions);
        queryWallet = queryWallet.GetWithSearch(searchWalletModel);
        // Apply sort
        queryWallet = queryWallet.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryWallet = queryWallet.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetWalletDetail>(queryWallet);
        return result.ToList();
    }

    public async Task<GetWalletDetail> GetWalletById(Guid id)
    {
        Wallet wallet = await _walletRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (wallet == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetWalletDetail>(wallet);
        return result;
    }

    public async Task<GetWalletDetail> CreateWalletAsync(CreateWalletModel requestBody)
    {
        Wallet wallet = _mapper.Map<Wallet>(requestBody);
        if (wallet == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        wallet.Balance = requestBody.Balance / _exchangeRate;
        wallet.Status = (int?)WalletEnum.WalletStatus.Active;
        await _walletRepository.InsertAsync(wallet);
        await _walletRepository.SaveChangesAsync();
        GetWalletDetail walletDetail = _mapper.Map<GetWalletDetail>(wallet);
        return walletDetail;
    }

    public async Task<GetWalletDetail> UpdateWalletAsync(Guid companyId, UpdateWalletModel requestBody)
    {
        if (companyId != requestBody.CompanyId)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var wallet = await _walletRepository.GetFirstOrDefaultAsync(alu => alu.CompanyId == requestBody.CompanyId);
        var currentBalance = wallet.Balance;
        if (wallet != null)
        {
            wallet = _mapper.Map(requestBody, wallet);
            wallet.Balance = (requestBody.Balance / _exchangeRate) + currentBalance;
            _walletRepository.Update(wallet);
            await _walletRepository.SaveChangesAsync();
        }
        GetWalletDetail walletDetail = _mapper.Map<GetWalletDetail>(wallet);
        return walletDetail;
    }

    public async Task DeleteWalletAsync(Guid id)
    {
        Wallet wallet = await _walletRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (wallet == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        wallet.Status = (int?)WalletEnum.WalletStatus.Inactive;
        await _walletRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _walletRepository.GetAll().CountAsync();
    }
}