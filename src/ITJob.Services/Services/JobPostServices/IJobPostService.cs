using ITJob.Entity.Entities;
using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.JobPost;

namespace ITJob.Services.Services.JobPostServices;

public interface IJobPostService
{
    IList<GetJobPostDetail> GetJobPostPage(PagingParam<JobPostEnum.JobPostSort> paginationModel, SearchJobPostModel searchJobPostModel);
    public IList<GetJobPostDetail> GetJobPostLikePage(
        PagingParam<JobPostEnum.JobPostSort> paginationModel,
        SearchJobPostModel searchProfileApplicantModel, Guid profileApplicantId);
    public IList<GetJobPostDetail> GetJobPostProfileApplicantLikePage(
        PagingParam<JobPostEnum.JobPostSort> paginationModel,
        SearchJobPostModel searchProfileApplicantModel, Guid profileApplicantId);
    public Task<GetJobPostDetail> GetJobPostById(Guid id);
    public Task<GetJobPostDetail> CreateJobPostAsync(CreateJobPostModel requestBody);
    public Task<GetJobPostDetail> UpdateJobPostAsync(Guid id, UpdateJobPostModel requestBody);
    public Task<GetJobPostDetail> UpdateJobPostExpiredAsync(Guid id, UpdateJobPostExpriredModel requestBody);
    public Task<GetJobPostDetail> UpdateJobPostMoneyAsync(Guid id, UpdateJobPostMoneyModel requestBody);
    public Task<GetJobPostDetail> ApprovalJobPostAsync(Guid id, ApprovalJobPostModel requestBody);
    public Task DeleteJobPostAsync(Guid id);
    public Task<int> GetTotal();
    public Task StartJob();
    public Task OutOfDateJob();
    public Task OutOfMoney();
}