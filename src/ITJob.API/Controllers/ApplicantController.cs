using ITJob.Services.Enum;
using ITJob.Services.Services.ApplicantServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.Applicant;
using ITJob.Services.ViewModels.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/applicants")]
public class ApplicantController : ControllerBase
{
    private readonly IApplicantService _applicantService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="applicantService"></param>
    public ApplicantController(IApplicantService applicantService)
    {
        _applicantService = applicantService;
    }
    
    /// <summary>
    /// [Guest] Endpoint for get all applicant with condition
    /// </summary>
    /// <param name="searchApplicantModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of applicant</returns>
    /// <response code="200">Returns the list of applicant</response>
    /// <response code="204">Returns if list of applicant is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetApplicantDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllApplicant(
        [FromQuery]PagingParam<ApplicantEnum.ApplicantSort> paginationModel, 
        [FromQuery]SearchApplicantModel searchApplicantModel)
    {
        IList<GetApplicantDetail> result = _applicantService.GetApplicantPage(paginationModel, searchApplicantModel);
        int total = await _applicantService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetApplicantDetail>()
        {
            Code = StatusCodes.Status200OK,
            Data = result.ToList(),
            Msg = "Use API get applicant page success!",
            Paging = new PagingMetadata()
            {
                Page = paginationModel.Page,
                Size = paginationModel.PageSize,
                Total = total
            },
        });
    }
    
    /// <summary>
    /// [Guest] Endpoint for get applicant by ID
    /// </summary>
    /// <param name="id">An id of applicant</param>
    /// <returns>List of applicant</returns>
    /// <response code="200">Returns the applicant</response>
    /// <response code="204">Returns if the applicant is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetApplicantDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetApplicantById(Guid id)
    {
        GetApplicantDetail result = await _applicantService.GetApplicantById(id);
        return Ok(new BaseResponse<GetApplicantDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get applicant by id success!",
            Data = result
        });
    }
    
    /// <summary>
    /// [Guest] Endpoint for get applicant by phone
    /// </summary>
    /// <param name="phone">An phone of applicant</param>
    /// <returns>An applicant</returns>
    /// <response code="200">Returns the applicant</response>
    /// <response code="204">Returns if the applicant is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("phone")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetApplicantDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetApplicantByPhone(string phone)
    {
        GetApplicantDetail result = await _applicantService.GetApplicantByPhone(phone);
        return Ok(new BaseResponse<GetApplicantDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get applicant by phone success!",
            Data = result
        });
    }
    
    /// <summary>
    /// [Applicant] Endpoint for create applicant
    /// </summary>
    /// <param name="requestBody">An obj contains input info of a applicant.</param>
    /// <returns>A applicant within status 201 or error status.</returns>
    /// <response code="201">Returns the applicant</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetApplicantDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateApplicant([FromForm] CreateApplicantModel requestBody)
    {
        var result = await _applicantService.CreateApplicantAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetApplicantDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Applicant] Endpoint for edit applicant.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of a applicant.</param>
    /// <returns>A applicant within status 200 or error status.</returns>
    /// <response code="200">Returns applicant after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("{id}")]
    [Authorize(Roles ="APPLICANT")]
    [ProducesResponseType(typeof(BaseResponse<GetApplicantDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateApplicantAsync(Guid id, [FromForm] UpdateApplicantModel requestBody)
    {
        try
        {
            GetApplicantDetail updateApplicant = await _applicantService.UpdateApplicantAsync(id,requestBody);

            return Ok(new BaseResponse<GetApplicantDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateApplicant,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }

    /// <summary>
    /// [Applicant] Endpoint for edit applicant for earn money.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody"></param>
    /// <returns>A applicant within status 200 or error status.</returns>
    /// <response code="200">Returns applicant after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("update")]
    [Authorize(Roles ="ADMIN,APPLICANT")]
    [ProducesResponseType(typeof(BaseResponse<GetApplicantDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateApplicantToEarn(Guid id, UpdateEarnMoneyApplicantModel requestBody)
    {
        try
        {
            string result = await _applicantService.UpdateApplicantToEarn(id, requestBody);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    
    /// <summary>
    /// [Applicant] Endpoint for applicant edit password.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="currentPassword"></param>
    /// <param name="newPassword"></param>
    /// <returns>A applicant within status 200 or error status.</returns>
    /// <response code="200">Returns applicant after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("password")]
    [Authorize(Roles ="APPLICANT")]
    [ProducesResponseType(typeof(BaseResponse<GetApplicantDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePasswordApplicantAsync(Guid id, string currentPassword, string newPassword)
    {
        try
        {
             string result = await _applicantService.UpdatePasswordApplicantAsync(id, currentPassword, newPassword);
             return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    /// <summary>
    /// [Applicant] Endpoint for applicant reset password.
    /// </summary>
    /// <param name="otp"></param>
    /// <param name="newPassword"></param>
    /// <param name="phone"></param>
    /// <returns>A applicant within status 200 or error status.</returns>
    /// <response code="200">Returns applicant after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("reset")]
    // [Authorize(Roles ="APPLICANT")]
    [ProducesResponseType(typeof(BaseResponse<GetApplicantDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgetPasswordApplicantAsync(string phone, int otp, string newPassword)
    {
        try
        {
            string result = await _applicantService.ForgetPasswordApplicantAsync(phone, otp, newPassword);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    /// <summary>
    /// [Admin] Endpoint for Admin Delete an applicant.
    /// </summary>
    /// <param name="id">ID of applicant</param>
    /// <param name="updateReason"></param>
    /// <returns>An applicant within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    [Authorize(Roles ="ADMIN")]
    public async Task<IActionResult> DeleteClassAsync(Guid id, UpdateReason updateReason)
    {
        try
        {
            await _applicantService.DeleteApplicantAsync(id, updateReason);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}