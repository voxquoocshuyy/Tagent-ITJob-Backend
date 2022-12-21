using ITJob.Services.ViewModels.User;

namespace ITJob.Services.Services.UserServices;

public interface IUserService
{

    // public String Login(String email);
    public string LoginApplicant(LoginApplicantModel loginApplicantModel);
    public Task<string> Login(LoginEmailModel loginCompanyModel);
}