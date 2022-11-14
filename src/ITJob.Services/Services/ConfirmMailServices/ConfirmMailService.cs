using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.AlbumImageRepositories;
using ITJob.Entity.Repositories.ApplicantRepositories;
using ITJob.Entity.Repositories.CompanyRepositories;
using ITJob.Entity.Repositories.UserRepositories;
using ITJob.Entity.Repositories.WalletRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.ViewModels.Company;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ITJob.Services.Services.ConfirmMailServices;

public class ConfirmMailService : IConfirmMailService
{
    private readonly IConfiguration _config;
    private readonly ICompanyRepository _companyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWalletRepository _walletRepository;

    public ConfirmMailService(IConfiguration configuration, IUserRepository userRepository, ICompanyRepository companyRepository,
        IWalletRepository walletRepository)
    {
        _config = configuration;
        _userRepository = userRepository;
        _companyRepository = companyRepository;
        _walletRepository = walletRepository;
    }

    public async Task<string> SendMailToAdminForCreateCompany(string email)
    {
        Company company = await _companyRepository.GetFirstOrDefaultAsync(c => c.Email == email);
        var emailInDb = company.Email;
        var name = company.Name;
        var phone = company.Phone;
        var website = company.Website;
        var taxCode = company.TaxCode;
        var logo = company.Logo;
        var info = "<strong>Email: </strong> " + emailInDb + "<br>" +
                   "<strong>Name: </strong> " + name + "<br>" +
                   "<strong>Phone: </strong> " + phone + "<br>" +
                   "<strong>Website: </strong> " + website + "<br>" +
                   "<strong>Tax code: </strong> " + taxCode + "<br>" +
                   "<img src=" + '"' + logo + '"' + ">";
        var apiKey = _config["Twilio:MyAPIKey"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_config["Twilio:MyTwilioEmail"], "Quoc Huy");
        var subject = "Sending by IT Job ";
        var to = new EmailAddress(_config["Twilio:MyAdminEmail"], "Admin");
        var plainTextContent = "This email for confirm your account company ";
        var content = plainTextContent + info;
        var htmlContent = "<strong>This email for confirm your company: </strong> " + info + "<br>" +
                          "<strong>Please check information of this company: </strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, content , htmlContent);
        await client.SendEmailAsync(msg);
        return "send mail to " + email + " success!!!";
    }
    public async Task<string> SendMailToAdminForCreateUser(string email)
    {
        var apiKey = _config["Twilio:MyAPIKey"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_config["Twilio:MyTwilioEmail"], "Quoc Huy");
        var subject = "Sending by IT Job ";
        var to = new EmailAddress(_config["Twilio:MyAdminEmail"], "Admin");
        var plainTextContent = "This email for confirm your account user "+ email +" want to create account";
        var htmlContent = "<strong>This email for confirm your account user "+ email +" want to create account. </strong> <br>" +
                          "<strong>Please check information of this user!!! </strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent , htmlContent);
        await client.SendEmailAsync(msg);
        return "send mail to admin success!!!";
    }
    public async Task<string> SendMailToAdminForApplicantEarn(string email)
    {
        var apiKey = _config["Twilio:MyAPIKey"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_config["Twilio:MyTwilioEmail"], "Quoc Huy");
        var subject = "Sending by IT Job ";
        var to = new EmailAddress(_config["Twilio:MyAdminEmail"], "Admin");
        var plainTextContent = "This email for confirm your applicant user"+ email +" want to make money ";
        var htmlContent = "<strong>This email for confirm your applicant user"+ email +" want to make money. </strong> <br>" +
                          "<strong>Please check information of this applicant!!! </strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent , htmlContent);
        await client.SendEmailAsync(msg);
        return "send mail to admin success!!!";
    }
    public async Task<string> SendMailToCompany(string email)
    {
        var min = 1111;
        var max = 9999;
        Random rdm = new Random();
        var code = rdm.Next(min, max);
        User user = await _userRepository.GetFirstOrDefaultAsync(c => c.Email == email);
        user.Code = code;
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
        var apiKey = _config["Twilio:MyAPIKey"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_config["Twilio:MyTwilioEmail"], "Quoc Huy");
        var subject = "Sending by IT Job ";
        var to = new EmailAddress(email, "Company");
        var plainTextContent = "This email for confirm your email";
        var htmlContent = "<strong>This email for confirm your email. </strong>"
                          + "Please verify this email by this code: </br> " + code;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent , htmlContent);
        await client.SendEmailAsync(msg);
        return "send mail to " + email + " success!!!";
    }
    public async Task<string> SendMailToCompanyForSuccess(string email)
    {
        var apiKey = _config["Twilio:MyAPIKey"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_config["Twilio:MyTwilioEmail"], "Quoc Huy");
        var subject = "Sending by IT Job ";
        var to = new EmailAddress(email, "Company");
        var plainTextContent = "This email for congratulations you have successfully created an account ";
        var htmlContent = "<strong>This email for congratulations you have successfully created an account </strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent , htmlContent);
        await client.SendEmailAsync(msg);
        return "send mail to " + email + " success!!!";
    }
    public async Task<string> SendMailToCompanyForFail(string email)
    {
        var apiKey = _config["Twilio:MyAPIKey"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_config["Twilio:MyTwilioEmail"], "Quoc Huy");
        var subject = "Sending by IT Job ";
        var to = new EmailAddress(email, "Company");
        var plainTextContent = "This email for sorry you don't have enough information to create an account ";
        var htmlContent = "<strong>This email for sorry you don't have enough information to create an account  </strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent , htmlContent);
        await client.SendEmailAsync(msg);
        return "send mail to " + email + " success!!!";
    }
    
    public async Task<string> SendMailToUserForJoinSuccess(string email)
    {
        var apiKey = _config["Twilio:MyAPIKey"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_config["Twilio:MyTwilioEmail"], "Quoc Huy");
        var subject = "Sending by IT Job ";
        var to = new EmailAddress(email, "Company");
        var plainTextContent = "This email for congratulations you have successfully joined to your company ";
        var htmlContent = "<strong>This email for congratulations you have successfully joined to your company </strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent , htmlContent);
        await client.SendEmailAsync(msg);
        return "send mail to " + email + " success!!!";
    }
    public async Task<string> SendMailToUserForJoinFail(string email)
    {
        var apiKey = _config["Twilio:MyAPIKey"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_config["Twilio:MyTwilioEmail"], "Quoc Huy");
        var subject = "Sending by IT Job ";
        var to = new EmailAddress(email, "Company");
        var plainTextContent = "This email for sorry you don't have enough information to join to your company ";
        var htmlContent = "<strong>This email for sorry you don't have enough information to join to your company  </strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent , htmlContent);
        await client.SendEmailAsync(msg);
        return "send mail to " + email + " success!!!";
    }
    public async Task<string> VerifyEmail(int code, string email)
    {
        User user = await _userRepository.GetFirstOrDefaultAsync(c => c.Email == email);
        if (user.Code == code)
        {
            user.Status = (int)UserEnum.UserStatus.Verified;
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }
        else
        {
            throw new CException(StatusCodes.Status400BadRequest, "Invalid code!!! ");
        }
        return "Verify success!!!";
    }
    public async Task<string> ConfirmCreateCompany(string email)
    {
        Company company = await _companyRepository.GetFirstOrDefaultAsync(c => c.Email == email);
        company.Status = (int)CompanyEnum.CompanyStatus.Active;
        _companyRepository.Update(company);
        await _companyRepository.SaveChangesAsync();
        var wallet = new Wallet
        {
            Balance = 0,
            Status = (int?)WalletEnum.WalletStatus.Active,
            CompanyId = company.Id,
        };
        await _walletRepository.InsertAsync(wallet);
        await _walletRepository.SaveChangesAsync();
        return "Confirm success!!!";
    }
    public async Task<string> RejectCreateCompany(string email)
    {
        Company company = await _companyRepository.GetFirstOrDefaultAsync(u => u.Email == email);
        _companyRepository.Delete(company);
        await _companyRepository.SaveChangesAsync();
        return "Reject success!!!";
    }
    
    public async Task<string> ConfirmJoinCompany(string email)
    {
        User user = await _userRepository.GetFirstOrDefaultAsync(c => c.Email == email);
        user.Status = (int)UserEnum.UserStatus.Active;
        _userRepository.Update(user);
        await _companyRepository.SaveChangesAsync();
        return "Confirm success!!!";
    }
    public async Task<string> RejectJoinCompany(string email)
    {
        User user = await _userRepository.GetFirstOrDefaultAsync(u => u.Email == email);
        user.Status = (int)UserEnum.UserStatus.Inactive;
        await _companyRepository.SaveChangesAsync();
        return "Reject success!!!";
    }
}