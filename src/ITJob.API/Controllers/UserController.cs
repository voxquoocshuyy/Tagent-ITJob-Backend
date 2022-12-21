using FirebaseAdmin.Auth;
using ITJob.Services.Enum;
using ITJob.Services.Services.UserServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.Applicant;
using ITJob.Services.ViewModels.Company;
using ITJob.Services.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // /// <summary>
    // /// [Guest] Endpoint for Guest login.
    // /// </summary>
    // /// <param name="loginModel">An obj contains phone and password user.</param>
    // /// <returns>A user within status 200 or 204 status.</returns>
    // /// <response code="200">Returns 200 status</response>
    // /// <response code="204">Returns NoContent status</response>
    // [Route("login")]
    // [HttpPost]
    // public async Task<IActionResult> Login(LoginModel loginModel)
    // {
    //     FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(loginModel.Token);
    //     string uid = decodedToken.Uid;
    //     UserRecord user = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
    //
    //     var response = _userService.Login(user.Email);
    //     if (response == null)
    //     {
    //         return BadRequest(new { message = "Token is invalid" });
    //     }
    //     return Ok(new { token = response });
    // }

    /// <summary>
    /// [Guest] Endpoint for applicant login.
    /// </summary>
    /// <param name="loginApplicantModel">An obj contains phone and password user.</param>
    /// <returns>A user within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [Route("login/applicant")]
    [HttpPost]
    public ActionResult LoginApplicant(LoginApplicantModel loginApplicantModel)
    {
        var response = _userService.LoginApplicant(loginApplicantModel);
        if (response == null)
        {
            return BadRequest(new { message = "Phone or password not correct" });
        }
        return Ok(new { token = response });
    }
    
    /// <summary>
    /// [Guest] Endpoint for Guest login.
    /// </summary>
    /// <param name="loginEmailModel">An obj contains phone and password user.</param>
    /// <returns>A user within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [Route("login")]
    [HttpPost]
    public async Task<ActionResult> Login(LoginEmailModel loginEmailModel)
    {
        var response = await _userService.Login(loginEmailModel);
        
        return Ok(new { token = response });
    }
}