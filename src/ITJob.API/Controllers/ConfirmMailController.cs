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
public class ConfirmMailController : ControllerBase
{
    private readonly IConfirmMailService _confirmMailService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="confirmMailService"></param>
    public ConfirmMailController(IConfirmMailService confirmMailService)
    {
        _confirmMailService = confirmMailService;
    }
    
    /// <summary>
    /// [Guest] Endpoint for confirm company
    /// </summary>
    /// <returns>An msg</returns>
    /// <response code="200">Returns the msg</response>
    /// <response code="204">Returns if the email is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("accept/email")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmCreateCompany(string email)
    {
        string result = await _confirmMailService.ConfirmCreateCompany(email);
        await _confirmMailService.SendMailToCompanyForSuccess(email);
        return Ok(result);
    }
    
    /// <summary>
    /// [Guest] Endpoint for reject company
    /// </summary>
    /// <returns>An msg</returns>
    /// <response code="200">Returns the msg</response>
    /// <response code="204">Returns if the email is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("reject/email")]
    [AllowAnonymous]
    public async Task<IActionResult> RejectCreateCompany(string email)
    {
        string result = await _confirmMailService.SendMailToCompanyForFail(email);
        await _confirmMailService.RejectCreateCompany(email);
        return Ok(result);
    }
    
    /// <summary>
    /// [Guest] Endpoint for confirm join company
    /// </summary>
    /// <returns>An msg</returns>
    /// <response code="200">Returns the msg</response>
    /// <response code="204">Returns if the email is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("accept/join")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmJoinCompany(string email)
    {
        string result = await _confirmMailService.ConfirmJoinCompany(email);
        await _confirmMailService.SendMailToUserForJoinSuccess(email);
        return Ok(result);
    }
    
    /// <summary>
    /// [Guest] Endpoint for reject join company
    /// </summary>
    /// <returns>An msg</returns>
    /// <response code="200">Returns the msg</response>
    /// <response code="204">Returns if the email is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("reject/join")]
    [AllowAnonymous]
    public async Task<IActionResult> RejectJoinCompany(string email)
    {
        string result = await _confirmMailService.RejectJoinCompany(email);
        await _confirmMailService.SendMailToUserForJoinFail(email);
        return Ok(result);
    }
    
    /// <summary>
    /// [Guest] Endpoint for send mail for confirm email
    /// </summary>
    /// <param name="email">An email of company</param>
    /// <returns>An OTP</returns>
    /// <response code="200">Returns the msg</response>
    /// <response code="204">Returns if the email is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost("mail-user")]
    [AllowAnonymous]
    public async Task<IActionResult> SendMailToCompany(string email)
    {
        string result = await _confirmMailService.SendMailToCompany(email);
        return Ok(result);
    }
    /// <summary>
    /// [Guest] Endpoint for send mail to admin want to join company
    /// </summary>
    /// <returns>An OTP</returns>
    /// <response code="200">Returns the msg</response>
    /// <response code="204">Returns if the email is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost("mail-join")]
    [AllowAnonymous]
    public async Task<IActionResult> SendMailToAdminToJoinCompany(string email)
    {
        string result = await _confirmMailService.SendMailToAdminForCreateUser(email);
        return Ok(result);
    }
    /// <summary>
    /// [Guest] Endpoint for send mail to admin fof applicant want to earn
    /// </summary>
    /// <returns>An OTP</returns>
    /// <response code="200">Returns the msg</response>
    /// <response code="204">Returns if the email is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost("mail-earn")]
    [AllowAnonymous]
    public async Task<IActionResult> SendMailToAdminForApplicantEarn(string email)
    {
        string result = await _confirmMailService.SendMailToAdminForApplicantEarn(email);
        return Ok(result);
    }
    /// <summary>
    /// [Guest] Endpoint for send mail confirm account company
    /// </summary>
    /// <param name="email">An email of company</param>
    /// <returns>A msg</returns>
    /// <response code="200">Returns the msg</response>
    /// <response code="204">Returns if the email is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SendMailToAdminForCreateCompany(string email)
    {
        string result = await _confirmMailService.SendMailToAdminForCreateCompany(email);
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
        string result = await _confirmMailService.VerifyEmail(code, email);
        return Ok(result);
    }
}