using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.SystemWallet;
using ITJob.Services.ViewModels.Transaction;

namespace ITJob.Services.Services.TransactionServices;

public interface ITransactionService
{
    IList<GetTransactionDetail> GetTransactionPage(PagingParam<TransactionEnum.TransactionSort> paginationModel,
        SearchTransactionModel searchTransactionModel);

    public Task<GetTransactionDetail> GetTransactionById(Guid id);
    public Task<GetSystemWalletDetail> GetSystemWallet();
    public Task<GetTransactionDetail> CreateTransactionAsync(CreateTransactionModel requestBody);

    public Task<GetTransactionDetail> UpdateTransactionAsync(Guid id, UpdateTransactionModel requestBody);

    public Task DeleteTransactionAsync(Guid id);

    public Task<int> GetTotal();
}