using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.JobPost;
using ITJob.Services.ViewModels.ProfileApplicant;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.Services.Services.MatchServices;
public interface IMatchService
{
    public Task<IQueryable<GetJobPostDetail>> CalculatorTotalScoreForProfileApplicantFilter(Guid profileApplicantId,
        PagingParam<JobPostEnum.JobPostSort> paginationModel, SearchJobPostModel searchJobPostModel);
    
    public Task<IQueryable<GetProfileApplicantDetail>> CalculatorTotalScoreForJobPostFilter(Guid jobPostId,
        PagingParam<ProfileApplicantEnum.ProfileApplicantSort> paginationModel, SearchProfileApplicantModel searchProfileApplicantModel);

    // public Task<IQueryable<GetProfileApplicantDetail>> CalculatorTotalScoreForJobPostFilterLike(Guid jobPostId,
    //     PagingParam<ProfileApplicantEnum.ProfileApplicantSort> paginationModel,
    //     SearchProfileApplicantModel searchProfileApplicantModel);
}