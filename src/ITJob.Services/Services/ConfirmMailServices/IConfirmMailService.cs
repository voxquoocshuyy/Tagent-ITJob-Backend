namespace ITJob.Services.Services.ConfirmMailServices;

public interface IConfirmMailService
{
    public Task<string> SendMailToAdminForCreateCompany(string email);
    public Task<string> SendMailToAdminForCreateUser(string email);
    public Task<string> SendMailToAdminForApplicantEarn(string email);
    public Task<string> SendMailToCompany(string email);
    public Task<string> VerifyEmail(int code, string email);
    public Task<string> SendMailToCompanyForSuccess(string email);
    public Task<string> SendMailToCompanyForFail(string email);
    public Task<string> ConfirmCreateCompany(string email);
    public Task<string> RejectCreateCompany(string email);
    public Task<string> ConfirmJoinCompany(string email);
    public Task<string> RejectJoinCompany(string email);
    public Task<string> SendMailToUserForJoinSuccess(string email);
    public Task<string> SendMailToUserForJoinFail(string email);

}