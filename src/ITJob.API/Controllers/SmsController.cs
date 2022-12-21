using ITJob.Services.Services.SendSMSServices;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.AlbumImage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/SMS")]
public class SmsController : ControllerBase
{
    private readonly ISendSMSService _sendSmsService;

    public SmsController(ISendSMSService sendSmsService)
    {
        _sendSmsService = sendSmsService;
    }
    /// <summary>
    /// [Guest] Endpoint for get OTP by phone
    /// </summary>
    /// <param name="phone">An phone of applicant</param>
    /// <returns>An OTP</returns>
    /// <response code="200">Returns the OTP</response>
    /// <response code="204">Returns if the OTP is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{phone}")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateOtp(string phone)
    {
        string result = await _sendSmsService.SendSms(phone);
        return Ok(result);
    }

    /// <summary>
    /// [Guest] Endpoint for verify OTP by phone
    /// </summary>
    /// <param name="code">An OTP have sent to your phone number </param>
    /// <param name="phone">A phone of applicant</param>
    /// <returns>An OTP</returns>
    /// <response code="200">Returns the OTP</response>
    /// <response code="204">Returns if the OTP is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyOtp(int code, string phone)
    {
        string result = await _sendSmsService.Verify(code, phone);
        return Ok(result);
    }
}