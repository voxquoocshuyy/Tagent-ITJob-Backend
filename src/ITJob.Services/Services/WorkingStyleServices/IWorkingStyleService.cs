using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.WorkingStyle;

namespace ITJob.Services.Services.WorkingStyleServices;

public interface IWorkingStyleService
{
    IList<GetWorkingStyleDetail> GetWorkingStylePage(PagingParam<WorkingStyleEnum.WorkingStyleSort> paginationModel,
        SearchWorkingStyleModel searchWorkingStyleModel);

    public Task<GetWorkingStyleDetail> GetWorkingStyleById(Guid id);

    public Task<GetWorkingStyleDetail> CreateWorkingStyleAsync(CreateWorkingStyleModel requestBody);

    public Task<GetWorkingStyleDetail> UpdateWorkingStyleAsync(Guid id, UpdateWorkingStyleModel requestBody);

    public Task DeleteWorkingStyleAsync(Guid id);

    public Task<int> GetTotal();
}