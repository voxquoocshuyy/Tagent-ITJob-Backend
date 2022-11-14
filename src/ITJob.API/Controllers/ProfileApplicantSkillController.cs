using ITJob.Services.Enum;
using ITJob.Services.Services.ProfileApplicantSkillServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.ProfileApplicantSkill;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/profile-applicant-skills")]
public class ProfileApplicantSkillController : ControllerBase
{
    private readonly IProfileApplicantSkillService _profileApplicantSkillService;

    public ProfileApplicantSkillController(IProfileApplicantSkillService profileApplicantSkillService)
    {
        _profileApplicantSkillService = profileApplicantSkillService;
    }
    /// <summary>
    /// [Guest] Endpoint for get all profile applicant skill with condition
    /// </summary>
    /// <param name="searchProfileApplicantSkillModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of profile applicant skill</returns>
    /// <response code="200">Returns the list of profile applicant skill</response>
    /// <response code="204">Returns if list of profile applicant skill is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetProfileApplicantSkillDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProfileApplicantSkill(
        [FromQuery]PagingParam<ProfileApplicantSkillEnum.ProfileApplicantSkillSort> paginationModel, 
        [FromQuery]SearchProfileApplicantSkillModel searchProfileApplicantSkillModel)
    {
        IList<GetProfileApplicantSkillDetail> result = _profileApplicantSkillService.GetProfileApplicantSkillPage(paginationModel, searchProfileApplicantSkillModel);
        int total = await _profileApplicantSkillService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetProfileApplicantSkillDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get profile applicant skill page success!",
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
    /// [Guest] Endpoint for get profile applicant skill by ID
    /// </summary>
    /// <param name="id">An id of profile applicant skill</param>
    /// <returns>List of profile applicant skill</returns>
    /// <response code="200">Returns the profile applicant skill</response>
    /// <response code="204">Returns if the profile applicant skill is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetProfileApplicantSkillDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfileApplicantSkillById(Guid id)
    {
        GetProfileApplicantSkillDetail result = await _profileApplicantSkillService.GetProfileApplicantSkillById(id);

        return Ok(new BaseResponse<GetProfileApplicantSkillDetail>()
        {
            Code = StatusCodes.Status200OK,
            Data = result,
            Msg = "Send Request Successful"
        });
    }
    
    /// <summary>
    /// [Admin] Endpoint for create profile applicant skill
    /// </summary>
    /// <param name="requestBody">An obj contains input info of an profile applicant skill.</param>
    /// <returns>A profile applicant skill within status 201 or error status.</returns>
    /// <response code="201">Returns the profile applicant skill</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetProfileApplicantSkillDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateProfileApplicantSkill([FromBody] CreateProfileApplicantSkillModel requestBody)
    {
        var result = await _profileApplicantSkillService.CreateProfileApplicantSkillAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetProfileApplicantSkillDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Admin] Endpoint for Admin edit profile applicant skill.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an profile applicant skill.</param>
    /// <returns>A profile applicant skill within status 200 or error status.</returns>
    /// <response code="200">Returns profile applicant skill after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetProfileApplicantSkillDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProfileApplicantSkillAsync(Guid id, [FromBody] UpdateProfileApplicantSkillModel requestBody)
    {
        try
        {
            GetProfileApplicantSkillDetail updateProfileApplicantSkill = await _profileApplicantSkillService.UpdateProfileApplicantSkillAsync(id, requestBody);

            return Ok(new BaseResponse<GetProfileApplicantSkillDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateProfileApplicantSkill,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    
    /// <summary>
    /// [Admin] Endpoint for Admin Delete a profile applicant skill.
    /// </summary>
    /// <param name="id">ID of profile applicant skill</param>
    /// <returns>A profile applicant skill within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _profileApplicantSkillService.DeleteProfileApplicantSkillAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}