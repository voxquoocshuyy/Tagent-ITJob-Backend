namespace ITJob.Services.Services.ConfirmMailServices;

public interface IMailService
{
    public Task<string> SendMailToAdminForCreateCompany(string email);
    public Task<string> SendMailToEmployeeForCreateAccount(string email, string password);
    public Task<string> SendMailToAdminForApplicantEarn(string email);
    public Task<string> SendMailForConfirmMail(string email);
    public Task<string> VerifyEmail(int code, string email);
    public Task<string> SendMailToCompanyForSuccess(string email);
    public Task<string> SendMailToCompanyForFail(string email);
    public Task<string> ConfirmCreateCompany(string email);
    public Task<string> RejectCreateCompany(string email);

}