using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.JobPostSkill;

namespace ITJob.Services.Services.JobPostSkillServices;

public interface IJobPostSkillService
{
    IList<GetJobPostSkillDetail> GetJobPostSkillPage(PagingParam<JobPostSkillEnum.JobPostSkillSort> paginationModel, SearchJobPostSkillModel searchJobPostSkillModel);

    public Task<GetJobPostSkillDetail> GetJobPostSkillById(Guid id);

    public Task<GetJobPostSkillDetail> CreateJobPostSkillAsync(CreateJobPostSkillModel requestBody);

    public Task<GetJobPostSkillDetail> UpdateJobPostSkillAsync(Guid id, UpdateJobPostSkillModel requestBody);

    public Task DeleteJobPostSkillAsync(Guid id);

    public Task<int> GetTotal();   
}