using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Applicant;
using ITJob.Services.ViewModels.User;

namespace ITJob.Services.Services.UserServices;

public interface IUserService
{
    IList<GetUserDetail> GetUserPage(PagingParam<UserEnum.UserSort> paginationModel,
        SearchUserModel searchUserModel);

    public Task<GetUserDetail> GetUserById(Guid id);
    public Task<GetUserDetail> GetUserByEmail(string email);

    public Task<GetUserDetail> CreateUserAsync(CreateUserModel requestBody);

    public Task<GetUserDetail> UpdateUserAsync(Guid id, UpdateUserModel requestBody);
    public Task<string> UpdatePasswordUserAsync(Guid id, string currentPassword, string newPassword);
    public Task<string> ForgetPasswordUserAsync(string email, int otp, string newPassword);
    public Task DeleteUserAsync(Guid id);

    public Task<int> GetTotal();

    public String Login(String email);
    
    public string LoginApplicant(LoginApplicantModel loginApplicantModel);
    public string LoginAdmin(LoginApplicantModel loginApplicantModel);
    public string LoginCompany(LoginCompanyModel loginCompanyModel);
}