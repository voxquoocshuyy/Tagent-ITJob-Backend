using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.ProfileApplicantSkill;

namespace ITJob.Services.Services.ProfileApplicantSkillServices;

public interface IProfileApplicantSkillService
{
    IList<GetProfileApplicantSkillDetail> GetProfileApplicantSkillPage(PagingParam<ProfileApplicantSkillEnum.ProfileApplicantSkillSort> paginationModel, SearchProfileApplicantSkillModel searchProfileApplicantSkillModel);

    public Task<GetProfileApplicantSkillDetail> GetProfileApplicantSkillById(Guid id);

    public Task<GetProfileApplicantSkillDetail> CreateProfileApplicantSkillAsync(CreateProfileApplicantSkillModel requestBody);

    public Task<GetProfileApplicantSkillDetail> UpdateProfileApplicantSkillAsync(Guid id, UpdateProfileApplicantSkillModel requestBody);

    public Task DeleteProfileApplicantSkillAsync(Guid id);

    public Task<int> GetTotal();   
}