using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.JobPosition;

namespace ITJob.Services.Services.JobPositionServices;

public interface IJobPositionService
{
    IList<GetJobPositionDetail> GetJobPositionPage(PagingParam<JobPositionEnum.JobPositionSort> paginationModel, SearchJobPositionModel searchJobPositionModel);

    public Task<GetJobPositionDetail> GetJobPositionById(Guid id);

    public Task<GetJobPositionDetail> CreateJobPositionAsync(CreateJobPositionModel requestBody);

    public Task<GetJobPositionDetail> UpdateJobPositionAsync(Guid id, UpdateJobPositionModel requestBody);

    public Task DeleteJobPositionAsync(Guid id);

    public Task<int> GetTotal();   
}