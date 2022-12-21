using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.ApplicantRepositories;
using ITJob.Entity.Repositories.UserRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Services.ApplicantServices;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.ViewModels.Applicant;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ITJob.Services.Services.SendSMSServices;

public class SendSMSService : ISendSMSService
{
    private readonly IConfiguration _config;
    private readonly IApplicantService _applicantService;
    private readonly IApplicantRepository _applicantRepository;
    private readonly IUserRepository _userRepository;
    public SendSMSService(IConfiguration config, IApplicantService applicantService, IApplicantRepository applicantRepository, IUserRepository userRepository)
    {
        _config = config;
        _applicantService = applicantService;
        _applicantRepository = applicantRepository;
        _userRepository = userRepository;
    }
    int min = 1111;
    int max = 9999;
    Random rdm = new Random();
    public async Task<string> SendSms(string phone)
    {
        GetApplicantDetail applicant = await _applicantService.GetApplicantByPhone(phone);
        string temptPhone = applicant.Phone;
        string convertPhone = temptPhone.Remove(0, 1);
        string phoneAfterConvert = "+84" + convertPhone;
        string otp = rdm.Next(min, max).ToString();
        var accountSid = _config["Twilio:AccountSid"];
        var authToken = _config["Twilio:AuthToken"];
        TwilioClient.Init(accountSid,authToken);
        var to = new PhoneNumber(phoneAfterConvert);
        var from = new PhoneNumber(_config["Twilio:MyTwilioPhone"]);
        var message = MessageResource.Create(
            to: to,
            from: from,
            body: otp);
        Applicant tempApplicant = await _applicantRepository.GetFirstOrDefaultAsync(a => a.Phone == phone);
        tempApplicant.Otp = Int32.Parse(otp);
        _applicantRepository.Update(tempApplicant);
        await _applicantRepository.SaveChangesAsync();
        return otp;
    }
    
    public async Task<string> Verify(int code, string phone)
    {
        var applicant = await _applicantRepository.GetFirstOrDefaultAsync(a => a.Phone == phone);
        var user = await _userRepository.GetFirstOrDefaultAsync(u => u.ApplicantId == applicant.Id);
        if (applicant.Otp == code)
        {
            applicant.Status = (int?)ApplicantEnum.ApplicantStatus.Active;
            _applicantRepository.Update(applicant);
            await _applicantRepository.SaveChangesAsync();
            
            user.Status = (int?)UserEnum.UserStatus.Active;
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }
        else
        {
            throw new CException(StatusCodes.Status400BadRequest, "Invalid code!!! ");
        }

        return "Verify success!!!";
    }
}