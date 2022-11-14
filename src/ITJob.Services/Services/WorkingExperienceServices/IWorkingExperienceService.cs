using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.WorkingExperience;

namespace ITJob.Services.Services.WorkingExperienceServices;

public interface IWorkingExperienceService
{
    IList<GetWorkingExperienceDetail> GetWorkingExperiencePage(PagingParam<WorkingExperienceEnum.WorkingExperienceSort> paginationModel,
        SearchWorkingExperienceModel searchWorkingExperienceModel);

    public Task<GetWorkingExperienceDetail> GetWorkingExperienceById(Guid id);

    public Task<GetWorkingExperienceDetail> CreateWorkingExperienceAsync(CreateWorkingExperienceModel requestBody);

    public Task<GetWorkingExperienceDetail> UpdateWorkingExperienceAsync(Guid id, UpdateWorkingExperienceModel requestBody);

    public Task DeleteWorkingExperienceAsync(Guid id);

    public Task<int> GetTotal();
}