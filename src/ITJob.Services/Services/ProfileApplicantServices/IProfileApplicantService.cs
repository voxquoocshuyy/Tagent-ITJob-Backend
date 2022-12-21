using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.ProfileApplicant;

namespace ITJob.Services.Services.ProfileApplicantServices;

public interface IProfileApplicantService
{
    IList<GetProfileApplicantDetail> GetProfileApplicantPage(PagingParam<ProfileApplicantEnum.ProfileApplicantSort> paginationModel, 
        SearchProfileApplicantModel searchProfileApplicantModel);
    IList<GetProfileApplicantDetail> GetProfileApplicantLikePage(PagingParam<ProfileApplicantEnum.ProfileApplicantSort> paginationModel,
        SearchProfileApplicantModel searchProfileApplicantModel, Guid jobPostId);
    IList<GetProfileApplicantDetail> GetProfileApplicantJobPostLikePage(
        PagingParam<ProfileApplicantEnum.ProfileApplicantSort> paginationModel,
        SearchProfileApplicantModel searchProfileApplicantModel, Guid jobPostId);
    public Task<GetProfileApplicantDetail> GetProfileApplicantById(Guid id);

    public Task<GetProfileApplicantDetail> CreateProfileApplicantAsync(CreateProfileApplicantModel requestBody);

    public Task<GetProfileApplicantDetail> UpdateProfileApplicantAsync(Guid id, UpdateProfileApplicantModel requestBody);

    public Task DeleteProfileApplicantAsync(Guid id);
    public Task ResetCount();
    public Task<int> GetTotal();   
}