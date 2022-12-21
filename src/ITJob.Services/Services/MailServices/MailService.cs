using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.CompanyRepositories;
using ITJob.Entity.Repositories.EmployeeRepositories;
using ITJob.Entity.Repositories.UserRepositories;
using ITJob.Entity.Repositories.WalletRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Services.ConfirmMailServices;
using ITJob.Services.Utility.ErrorHandling.Object;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ITJob.Services.Services.MailServices;

public class MailService : IMailService
{
    private readonly IConfiguration _config;
    private readonly ICompanyRepository _companyRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public MailService(IConfiguration configuration, IUserRepository userRepository, ICompanyRepository companyRepository,
        IWalletRepository walletRepository, IEmployeeRepository employeeRepository)
    {
        _config = configuration;
        _userRepository = userRepository;
        _companyRepository = companyRepository;
        _walletRepository = walletRepository;
        _employeeRepository = employeeRepository;
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
        var htmlContent ="<img src=" + '"' + _config["SystemConfiguration:Logo"] + '"' + ">" +"<br>"+
                         "<strong>Thank you for trusting and using tagent recruitment platform. </strong>" +"<br>" +
                         "<strong>This email for confirm your company: </strong> " + info + "<br>" +
                         "<strong>Please check information of this company: </strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, content , htmlContent);
        await client.SendEmailAsync(msg);
        return "send mail to " + email + " success!!!";
    }
    public async Task<string> SendMailToEmployeeForCreateAccount(string email, string password)
    {
        var employee = await _employeeRepository.GetFirstOrDefaultAsync(c => c.Email == email);
        var name = employee.Name;
        var phone = employee.Phone;
        var info = "<strong>Email: </strong> " + email + "<br>" +
                   "<strong>Name: </strong> " + name + "<br>" +
                   "<strong>Phone: </strong> " + phone + "<br>" +
                   "<strong>Password: </strong>"+ password + "<br>";
        var apiKey = _config["Twilio:MyAPIKey"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_config["Twilio:MyTwilioEmail"], "Quoc Huy");
        var subject = "Sending by IT Job ";
        var to = new EmailAddress(email, "Employee");
        var plainTextContent = "This email for confirm your account company ";
        var content = plainTextContent + info;
        var htmlContent = "<img src=" + '"' + _config["SystemConfiguration:Logo"] + '"' + ">" +"<br>"+
            "<strong>Thank you for trusting and using tagent recruitment platform. " +"<br>" +
            "Your employee account has been created: </strong> "+ "<br>" + info + "<br>" +
                          "<strong> You can login to Tagent IT with this email and password.</strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, content , htmlContent);
        await client.SendEmailAsync(msg);
        return "send mail to " + email + " success!!!";
    }
    public async Task<string> SendMailToAdminForApplicantEarn(string email)
    {
        var apiKey = _config["Twilio:MyAPIKey"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_config["Twilio:MyTwilioEmail"], "Quoc Huy");
        var subject = "Sending by IT Job ";
        var to = new EmailAddress(_config["Twilio:MyAdminEmail"], "Admin");
        var plainTextContent = "This email for confirm your applicant user"+ email +" want to make money ";
        var htmlContent ="<img src=" + '"' + _config["SystemConfiguration:Logo"] + '"' + ">" +"<br>"+
                         "<strong>Thank you for trusting and using tagent recruitment platform. </strong>" +"<br>" +
                         "<strong>This email for confirm your applicant user"+ email +" want to make money. </strong> <br>" +
                         "<strong>Please check information of this applicant!!! </strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent , htmlContent);
        await client.SendEmailAsync(msg);
        return "send mail to admin success!!!";
    }
    public async Task<string> SendMailForConfirmMail(string email)
    {
        var min = 1111;
        var max = 9999;
        Random rdm = new Random();
        var code = rdm.Next(min, max);
        var company = await _companyRepository.GetFirstOrDefaultAsync(c => c.Email == email);
        company.Code = code;
        _companyRepository.Update(company);
        await _companyRepository.SaveChangesAsync();
        
        var apiKey = _config["Twilio:MyAPIKey"];
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_config["Twilio:MyTwilioEmail"], "Quoc Huy");
        var subject = "Sending by IT Job ";
        var to = new EmailAddress(email, "Company");
        var plainTextContent = "This email for confirm your email";
        var htmlContent ="<img src=" + '"' + _config["SystemConfiguration:Logo"] + '"' + ">" +"<br>"+
                         "<strong>Thank you for trusting and using tagent recruitment platform. </strong>" +"<br>" +
                         "<strong>This email for confirm your email. </strong>"
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
        var plainTextContent = "This email for congratulations you have successfully created a company ";
        var htmlContent = "<img src=" + '"' + _config["SystemConfiguration:Logo"] + '"' + ">" +"<br>"+
                          "<strong>Thank you for trusting and using tagent recruitment platform. </strong>" +"<br>" +
                          "<strong>This email for congratulations you have successfully created a company </strong>";
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
        var plainTextContent = "This email for sorry you don't have enough information to create a company ";
        var htmlContent = "<img src=" + '"' + _config["SystemConfiguration:Logo"] + '"' + ">" +"<br>"+
                          "<strong>Thank you for trusting and using tagent recruitment platform. </strong>" +"<br>" +
                          "<strong>This email for sorry you don't have enough information to create a company  </strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent , htmlContent);
        await client.SendEmailAsync(msg);
        return "send mail to " + email + " success!!!";
    }
    public async Task<string> VerifyEmail(int code, string email)
    {
        var company = await _companyRepository.GetFirstOrDefaultAsync(c => c.Email == email);
        var user = await _userRepository.GetFirstOrDefaultAsync(u => u.CompanyId == company.Id);
        if (company.Code == code)
        {
            company.Status = (int)CompanyEnum.CompanyStatus.Pending;
            _companyRepository.Update(company);
            await _companyRepository.SaveChangesAsync();
            user.Status = (int)UserEnum.UserStatus.Pending;
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();     }
        else
        {
            throw new CException(StatusCodes.Status400BadRequest, "Invalid code!!! ");
        }
        return "Verify success!!!";
    }
    public async Task<string> ConfirmCreateCompany(string email)
    {
        User user = await _userRepository.GetFirstOrDefaultAsync(c => c.Email == email);
        Company company = await _companyRepository.GetFirstOrDefaultAsync(c => c.Id == user.CompanyId);
        company.Status = (int)CompanyEnum.CompanyStatus.Active;
        _companyRepository.Update(company);
        await _companyRepository.SaveChangesAsync();
        user.Status = (int)UserEnum.UserStatus.Active;
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();
        var wallet = new Wallet
        {
            Balance = (double)0,
            Status = (int?)WalletEnum.WalletStatus.Active,
            CompanyId = company.Id,
        };
        await _walletRepository.InsertAsync(wallet);
        await _walletRepository.SaveChangesAsync();
        return "Confirm success!!!";
    }
    public async Task<string> RejectCreateCompany(string email)
    {
        var user = await _userRepository.GetFirstOrDefaultAsync(c => c.Email == email);
        _userRepository.Delete(user);
        await _userRepository.SaveChangesAsync();
        
        var company = await _companyRepository.GetFirstOrDefaultAsync(u => u.Id == user.CompanyId);
        _companyRepository.Delete(company);
        await _companyRepository.SaveChangesAsync();
        return "Reject success!!!";
    }
}