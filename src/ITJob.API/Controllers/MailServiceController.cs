using ITJob.Services.Services.ConfirmMailServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/emails")]
public class MailServiceController : ControllerBase
{
    private readonly IMailService _mailService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mailService"></param>
    public MailServiceController(IMailService mailService)
    {
        _mailService = mailService;
    }
    
    /// <summary>
    /// [Guest] Endpoint for confirm create company
    /// </summary>
    /// <returns>An msg</returns>
    /// <response code="200">Returns the msg</response>
    /// <response code="204">Returns if the email is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("accept/email")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmCreateCompany(string email)
    {
        string result = await _mailService.ConfirmCreateCompany(email);
        await _mailService.SendMailToCompanyForSuccess(email);
        return Ok(result);
    }
    
    /// <summary>
    /// [Guest] Endpoint for reject create company
    /// </summary>
    /// <returns>An msg</returns>
    /// <response code="200">Returns the msg</response>
    /// <response code="204">Returns if the email is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("reject/email")]
    [AllowAnonymous]
    public async Task<IActionResult> RejectCreateCompany(string email)
    {
        string result = await _mailService.SendMailToCompanyForFail(email);
        await _mailService.RejectCreateCompany(email);
        return Ok(result);
    }
    
    /// <summary>
    /// [Guest] Endpoint for send mail to admin fof applicant want to earn
    /// </summary>
    /// <returns>An OTP</returns>
    /// <response code="200">Returns the msg</response>
    /// <response code="204">Returns if the email is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    /// 
    [HttpPost("mail-earn")]
    [AllowAnonymous]
    public async Task<IActionResult> SendMailToAdminForApplicantEarn(string email)
    {
        string result = await _mailService.SendMailToAdminForApplicantEarn(email);
        return Ok(result);
    }
    
    /// <summary>
    /// [Guest] Endpoint for send mail to admin for create account company
    /// </summary>
    /// <param name="email">An email of company</param>
    /// <returns>A msg</returns>
    /// <response code="200">Returns the msg</response>
    /// <response code="204">Returns if the email is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost("mail-create-company")]
    [AllowAnonymous]
    public async Task<IActionResult> SendMailToAdminForCreateCompany(string email)
    {
        string result = await _mailService.SendMailToAdminForCreateCompany(email);
        return Ok(result);
    }
    
    // /// <summary>
    // /// [Guest] Endpoint for send mail to employee for create account
    // /// </summary>
    // /// <param name="email">An email of employee</param>
    // /// <returns>A msg</returns>
    // /// <response code="200">Returns the msg</response>
    // /// <response code="204">Returns if the email is not exist</response>
    // /// <response code="403">Return if token is access denied</response>
    // [HttpPost("mail-create-employee")]
    // [AllowAnonymous]
    // public async Task<IActionResult> SendMailToEmployeeForCreateAccount(string email)
    // {
    //     string result = await _mailService.SendMailToEmployeeForCreateAccount(email);
    //     return Ok(result);
    // }
    /// <summary>
    /// [Guest] Endpoint for send mail confirm account company
    /// </summary>
    /// <param name="email">An email of company</param>
    /// <returns>A msg</returns>
    /// <response code="200">Returns the msg</response>
    /// <response code="204">Returns if the email is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost("mail-confirm-company")]
    [AllowAnonymous]
    public async Task<IActionResult> SendMailToCompanyForConfirmCompany(string email)
    {
        string result = await _mailService.SendMailForConfirmMail(email);
        return Ok(result);
    }
    
    /// <summary>
    /// [Guest] Endpoint for verify code
    /// </summary>
    /// <param name="code">A code have sent to your mail address </param>
    /// <param name="email">An email of user</param>
    /// <returns>A msg</returns>
    /// <response code="200">Returns the msg</response>
    /// <response code="204">Returns if the OTP is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost("otp")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyCode(int code, string email)
    {
        string result = await _mailService.VerifyEmail(code, email);
        return Ok(result);
    }
}