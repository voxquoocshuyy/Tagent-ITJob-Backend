using ITJob.Services.Enum;
using ITJob.Services.Services.MatchServices;
using ITJob.Services.Services.ProfileApplicantServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.ProfileApplicant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/profile-applicants")]
public class ProfileApplicantController : ControllerBase
{
    private readonly IProfileApplicantService _profileApplicantService;
    private readonly IMatchService _matchService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="profileApplicantService"></param>
    /// <param name="matchService"></param>
    public ProfileApplicantController(IProfileApplicantService profileApplicantService, IMatchService matchService)
    {
        _profileApplicantService = profileApplicantService;
        _matchService = matchService;
    }
    /// <summary>
    /// [Guest] Endpoint for get all profile applicant with condition
    /// </summary>
    /// <param name="searchProfileApplicantModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of profile applicant</returns>
    /// <response code="200">Returns the list of profile applicant</response>
    /// <response code="204">Returns if list of profile applicant is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetProfileApplicantDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProfileApplicant(
        [FromQuery]PagingParam<ProfileApplicantEnum.ProfileApplicantSort> paginationModel, 
        [FromQuery]SearchProfileApplicantModel searchProfileApplicantModel)
    {
        IList<GetProfileApplicantDetail> result = _profileApplicantService.GetProfileApplicantPage(paginationModel, searchProfileApplicantModel);
        int total = await _profileApplicantService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetProfileApplicantDetail>()
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
    /// [Guest] Endpoint for get all profile applicant with sort by system
    /// </summary>
    /// <param name="jobPostId"> An id of job post</param>
    /// <param name="paginationModel"></param>
    /// <param name="searchProfileApplicantModel"></param>
    /// <returns>List of profile applicant</returns>
    /// <response code="200">Returns the list of profile applicant</response>
    /// <response code="204">Returns if list of profile applicant is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("jobPostId")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetProfileApplicantDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProfileApplicantSortBySystem(Guid jobPostId,
        [FromQuery]PagingParam<ProfileApplicantEnum.ProfileApplicantSort> paginationModel, 
        [FromQuery]SearchProfileApplicantModel searchProfileApplicantModel)
    {
        var result = await _matchService.CalculatorTotalScoreForJobPostFilter(jobPostId,
            paginationModel, searchProfileApplicantModel);
        return Ok(new ModelsResponse<GetProfileApplicantDetail>
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get profile applicant sort by system success!",
            Data = result.ToList(),
            Paging = new PagingMetadata()
            {
                Page = paginationModel.Page,
                Size = paginationModel.PageSize,
                Total = result.ToList().Count
            },
        });
    }
    
    /// <summary>
    /// [Guest] Endpoint for get all profile applicant like job post with condition
    /// </summary>
    /// <param name="searchProfileApplicantModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <param name="jobPostId"></param>
    /// <returns>List of profile applicant</returns>
    /// <response code="200">Returns the list of profile applicant</response>
    /// <response code="204">Returns if list of profile applicant is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("like")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetProfileApplicantDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfileApplicantLike(
        [FromQuery]PagingParam<ProfileApplicantEnum.ProfileApplicantSort> paginationModel, 
        [FromQuery]SearchProfileApplicantModel searchProfileApplicantModel, Guid jobPostId)
    {
        var result = _profileApplicantService.GetProfileApplicantLikePage(paginationModel,
            searchProfileApplicantModel, jobPostId);

        return Ok(new ModelsResponse<GetProfileApplicantDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Send Request Successful",
            Data = result.ToList(),
            Paging = new PagingMetadata()
            {
                Page = paginationModel.Page,
                Size = paginationModel.PageSize,
                Total = result.ToList().Count
            },
        });
    }
    /// <summary>
    /// [Guest] Endpoint for get all profile applicant like job post with condition
    /// </summary>
    /// <param name="searchProfileApplicantModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <param name="jobPostId"></param>
    /// <returns>List of profile applicant</returns>
    /// <response code="200">Returns the list of profile applicant</response>
    /// <response code="204">Returns if list of profile applicant is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("jobPost-like")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetProfileApplicantDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfileApplicantJobPostLikePage(
        [FromQuery]PagingParam<ProfileApplicantEnum.ProfileApplicantSort> paginationModel, 
        [FromQuery]SearchProfileApplicantModel searchProfileApplicantModel, Guid jobPostId)
    {
        IList<GetProfileApplicantDetail> result = _profileApplicantService.GetProfileApplicantJobPostLikePage(paginationModel,
            searchProfileApplicantModel, jobPostId);
        int total = await _profileApplicantService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetProfileApplicantDetail>()
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
    /// [Guest] Endpoint for get profile applicant by ID
    /// </summary>
    /// <param name="id">An id of profile applicant</param>
    /// <returns>List of profile applicant</returns>
    /// <response code="200">Returns the profile applicant</response>
    /// <response code="204">Returns if the profile applicant is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetProfileApplicantDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfileApplicantById(Guid id)
    {
        GetProfileApplicantDetail result = await _profileApplicantService.GetProfileApplicantById(id);

        return Ok(new BaseResponse<GetProfileApplicantDetail>()
        {
            Code = StatusCodes.Status200OK,
            Data = result,
            Msg = "Send Request Successful"
        });
    }
    
    /// <summary>
    /// [Applicant] Endpoint for create profile applicant
    /// </summary>
    /// <param name="requestBody">An obj contains input info of an profile applicant.</param>
    /// <returns>A profile applicant within status 201 or error status.</returns>
    /// <response code="201">Returns the profile applicant</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="APPLICANT")]
    [ProducesResponseType(typeof(BaseResponse<GetProfileApplicantDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateProfileApplicant([FromBody] CreateProfileApplicantModel requestBody)
    {
        var result = await _profileApplicantService.CreateProfileApplicantAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetProfileApplicantDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Applicant] Endpoint for edit profile applicant.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an profile applicant.</param>
    /// <returns>A profile applicant within status 200 or error status.</returns>
    /// <response code="200">Returns profile applicant after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("id")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="APPLICANT")]
    [ProducesResponseType(typeof(BaseResponse<GetProfileApplicantDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProfileApplicantAsync(Guid id, [FromBody] UpdateProfileApplicantModel requestBody)
    {
        try
        {
            GetProfileApplicantDetail updateProfileApplicant = await _profileApplicantService.UpdateProfileApplicantAsync(id, requestBody);

            return Ok(new BaseResponse<GetProfileApplicantDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateProfileApplicant,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    
    /// <summary>
    /// [Applicant] Endpoint for delete a profile applicant.
    /// </summary>
    /// <param name="id">ID of profile applicant</param>
    /// <returns>A profile applicant within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="APPLICANT")]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _profileApplicantService.DeleteProfileApplicantAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}