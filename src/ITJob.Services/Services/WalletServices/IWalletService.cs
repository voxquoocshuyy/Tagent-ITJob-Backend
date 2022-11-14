using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Wallet;

namespace ITJob.Services.Services.WalletServices;

public interface IWalletService
{
    IList<GetWalletDetail> GetWalletPage(PagingParam<WalletEnum.WalletSort> paginationModel,
        SearchWalletModel searchWalletModel);

    public Task<GetWalletDetail> GetWalletById(Guid id);

    public Task<GetWalletDetail> CreateWalletAsync(CreateWalletModel requestBody);

    public Task<GetWalletDetail> UpdateWalletAsync(Guid companyId, UpdateWalletModel requestBody);

    public Task DeleteWalletAsync(Guid id);

    public Task<int> GetTotal();
}