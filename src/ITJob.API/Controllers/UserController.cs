using FirebaseAdmin.Auth;
using ITJob.Services.Enum;
using ITJob.Services.Services.UserServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.Applicant;
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
    /// <summary>
    /// [Guest] Endpoint for get all user with condition
    /// </summary>
    /// <param name="searchUserModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of user</returns>
    /// <response code="200">Returns the list of user</response>
    /// <response code="204">Returns if list of user is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetUserDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUser(
        [FromQuery]PagingParam<UserEnum.UserSort> paginationModel, 
        [FromQuery]SearchUserModel searchUserModel)
    {
        IList<GetUserDetail> result = _userService.GetUserPage(paginationModel, searchUserModel);
        int total = await _userService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetUserDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Send Request Successful",
            Data = result.ToList(),
            Paging = new PagingMetadata()
            {
                Page = paginationModel.Page,
                Size = paginationModel.PageSize,
                Total = total
            },
        });
    }
    
    /// <summary>
    /// [Guest] Endpoint for get user by ID
    /// </summary>
    /// <param name="id">An id of user</param>
    /// <returns>List of user</returns>
    /// <response code="200">Returns the user</response>
    /// <response code="204">Returns if the user is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetUserDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        GetUserDetail result = await _userService.GetUserById(id);
            
        if (result == null)
        {
            return NoContent();
        }
            
        return Ok(new BaseResponse<GetUserDetail>()
        {
            Code = StatusCodes.Status200OK,
            Data = result,
            Msg = "Send Request Successful"
        });
    }
    /// <summary>
    /// [Guest] Endpoint for get user by email
    /// </summary>
    /// <param name="email">An email of user</param>
    /// <returns>An user</returns>
    /// <response code="200">Returns the user</response>
    /// <response code="204">Returns if the user is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("email")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetUserDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        GetUserDetail result = await _userService.GetUserByEmail(email);
        return Ok(new BaseResponse<GetUserDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get user by email success!",
            Data = result
        });
    }
    /// <summary>
    /// [Admin] Endpoint for create user
    /// </summary>
    /// <param name="requestBody">An obj contains input info of an user.</param>
    /// <returns>A user within status 201 or error status.</returns>
    /// <response code="201">Returns the user</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetUserDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserModel requestBody)
    {
        var result = await _userService.CreateUserAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetUserDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Admin] Endpoint for Admin edit user.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an user.</param>
    /// <returns>A user within status 200 or error status.</returns>
    /// <response code="200">Returns user after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetUserDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUserAsync(Guid id, [FromBody] UpdateUserModel requestBody)
    {
        try
        {
            GetUserDetail updateUser = await _userService.UpdateUserAsync(id, requestBody);

            return Ok(new BaseResponse<GetUserDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateUser,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    /// <summary>
    /// [User] Endpoint for user edit password.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="currentPassword"></param>
    /// <param name="newPassword"></param>
    /// <returns>A applicant within status 200 or error status.</returns>
    /// <response code="200">Returns applicant after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("password")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetUserDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePasswordApplicantAsync(Guid id, string currentPassword, string newPassword)
    {
        try
        {
            string result = await _userService.UpdatePasswordUserAsync(id, currentPassword, newPassword);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    /// <summary>
    /// [User] Endpoint for user reset password.
    /// </summary>
    /// <param name="otp"></param>
    /// <param name="newPassword"></param>
    /// <param name="email"></param>
    /// <returns>A user within status 200 or error status.</returns>
    /// <response code="200">Returns user after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("reset")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetUserDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgetPasswordUserAsync(string email, int otp, string newPassword)
    {
        try
        {
            string result = await _userService.ForgetPasswordUserAsync(email, otp, newPassword);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    /// <summary>
    /// [Admin] Endpoint for Admin Delete a user.
    /// </summary>
    /// <param name="id">ID of user</param>
    /// <returns>A user within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _userService.DeleteUserAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
    
    /// <summary>
    /// [Guest] Endpoint for Guest login.
    /// </summary>
    /// <param name="requestBody">An obj contains phone and password user.</param>
    /// <returns>A user within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(loginModel.Token);
        string uid = decodedToken.Uid;
        UserRecord user = await FirebaseAuth.DefaultInstance.GetUserAsync(uid);
    
        var response = _userService.Login(user.Email);
        if (response == null)
        {
            return BadRequest(new { message = "Token is invalid" });
        }
        return Ok(new { token = response });
    }

    /// <summary>
    /// [Guest] Endpoint for Guest login.
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
    /// <param name="loginApplicantModel">An obj contains phone and password user.</param>
    /// <returns>A user within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [Route("login/company")]
    [HttpPost]
    public ActionResult LoginCompany(LoginCompanyModel loginCompanyModel)
    {
        var response = _userService.LoginCompany(loginCompanyModel);
        if (response == null)
        {
            return BadRequest(new { message = "Phone or password not correct" });
        }
        return Ok(new { token = response });
    }

    /// <summary>
    /// [Admin] Endpoint for admin login.
    /// </summary>
    /// <param name="loginApplicantModel">An obj contains phone and password user.</param>
    /// <returns>A user within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [Route("login/admin")]
    [HttpPost]
    public ActionResult LoginAdmin(LoginApplicantModel loginApplicantModel)
    {
        var response = _userService.LoginAdmin(loginApplicantModel);
        if (response == null)
        {
            return BadRequest(new { message = "Phone or password not correct" });
        }
        return Ok(new { token = response });
    }
}