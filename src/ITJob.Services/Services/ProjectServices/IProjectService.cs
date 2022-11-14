using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Project;

namespace ITJob.Services.Services.ProjectServices;

public interface IProjectService
{
    IList<GetProjectDetail> GetProjectPage(PagingParam<ProjectEnum.ProjectSort> paginationModel, SearchProjectModel searchProjectModel);

    public Task<GetProjectDetail> GetProjectById(Guid id);

    public Task<GetProjectDetail> CreateProjectAsync(CreateProjectModel requestBody);

    public Task<GetProjectDetail> UpdateProjectAsync(Guid id, UpdateProjectModel requestBody);

    public Task DeleteProjectAsync(Guid id);

    public Task<int> GetTotal();   
}