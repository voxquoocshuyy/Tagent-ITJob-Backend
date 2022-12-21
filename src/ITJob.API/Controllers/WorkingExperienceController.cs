using ITJob.Services.Enum;
using ITJob.Services.Services.WorkingExperienceServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.WorkingExperience;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/working-experiences")]
public class WorkingExperienceController : ControllerBase
{
    private readonly IWorkingExperienceService _workingExperienceService;

    public WorkingExperienceController(IWorkingExperienceService workingExperienceService)
    {
        _workingExperienceService = workingExperienceService;
    }
    
    /// <summary>
    /// [Guest] Endpoint for get all working experience with condition
    /// </summary>
    /// <param name="searchWorkingExperienceModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of working experience</returns>
    /// <response code="200">Returns the list of working experience</response>
    /// <response code="204">Returns if list of working experience is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetWorkingExperienceDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllWorkingExperience(
        [FromQuery]PagingParam<WorkingExperienceEnum.WorkingExperienceSort> paginationModel, 
        [FromQuery]SearchWorkingExperienceModel searchWorkingExperienceModel)
    {
        IList<GetWorkingExperienceDetail> result = _workingExperienceService.GetWorkingExperiencePage(paginationModel, searchWorkingExperienceModel);
        int total = await _workingExperienceService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetWorkingExperienceDetail>()
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
    /// [Guest] Endpoint for get working experience by ID
    /// </summary>
    /// <param name="id">An id of working experience</param>
    /// <returns>List of workingExperience</returns>
    /// <response code="200">Returns the working experience</response>
    /// <response code="204">Returns if the working experience is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetWorkingExperienceDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWorkingExperienceById(Guid id)
    {
        GetWorkingExperienceDetail result = await _workingExperienceService.GetWorkingExperienceById(id);

        return Ok(new BaseResponse<GetWorkingExperienceDetail>()
        {
            Code = StatusCodes.Status200OK,
            Data = result,
            Msg = "Send Request Successful"
        });
    }
    
    /// <summary>
    /// [Admin] Endpoint for create working experience
    /// </summary>
    /// <param name="requestBody">An obj contains input info of an working experience.</param>
    /// <returns>A working experience within status 201 or error status.</returns>
    /// <response code="201">Returns the working experience</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="APPLICANT")]
    [ProducesResponseType(typeof(BaseResponse<GetWorkingExperienceDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateWorkingExperience([FromBody] CreateWorkingExperienceModel requestBody)
    {
        var result = await _workingExperienceService.CreateWorkingExperienceAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetWorkingExperienceDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Admin] Endpoint for Admin edit working experience.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an working experience.</param>
    /// <returns>A working experience within status 200 or error status.</returns>
    /// <response code="200">Returns working experience after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="APPLICANT")]
    [ProducesResponseType(typeof(BaseResponse<GetWorkingExperienceDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateWorkingExperienceAsync(Guid id, [FromBody] UpdateWorkingExperienceModel requestBody)
    {
        try
        {
            GetWorkingExperienceDetail updateWorkingExperience = await _workingExperienceService.UpdateWorkingExperienceAsync(id, requestBody);

            return Ok(new BaseResponse<GetWorkingExperienceDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateWorkingExperience,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    
    /// <summary>
    /// [Admin] Endpoint for Admin Delete a working experience.
    /// </summary>
    /// <param name="id">ID of working experience</param>
    /// <returns>A working experience within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="APPLICANT")]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _workingExperienceService.DeleteWorkingExperienceAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }

}