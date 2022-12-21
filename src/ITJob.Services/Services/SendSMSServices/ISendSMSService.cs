namespace ITJob.Services.Services.SendSMSServices;

public interface ISendSMSService
{
    public Task<string> SendSms(string phone);
    public Task<string> Verify(int code, string phone);

}