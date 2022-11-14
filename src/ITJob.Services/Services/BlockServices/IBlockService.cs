using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Block;

namespace ITJob.Services.Services.BlockServices;

public interface IBlockService
{
    IList<GetBlockDetail> GetBlockPage(PagingParam<BlockEnum.BlockSort> paginationModel, SearchBlockModel searchBlockModel);

    public Task<GetBlockDetail> GetBlockById(Guid id);

    public Task<GetBlockDetail> CreateBlockAsync(CreateBlockModel requestBody);

    public Task<GetBlockDetail> UpdateBlockAsync(Guid id, UpdateBlockModel requestBody);

    public Task DeleteBlockAsync(Guid id);

    public Task<int> GetTotal();   
}