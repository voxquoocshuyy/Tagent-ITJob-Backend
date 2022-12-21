using ITJob.Services.Enum;
using ITJob.Services.Services.WorkingStyleServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.WorkingStyle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/working-styles")]
public class WorkingStyleController : ControllerBase
{
    private readonly IWorkingStyleService _workingStyleService;

    public WorkingStyleController(IWorkingStyleService workingStyleService)
    {
        _workingStyleService = workingStyleService;
    }
    /// <summary>
    /// [Guest] Endpoint for get all working style with condition
    /// </summary>
    /// <param name="searchWorkingStyleModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of working style</returns>
    /// <response code="200">Returns the list of working style</response>
    /// <response code="204">Returns if list of working style is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetWorkingStyleDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllWorkingStyle(
        [FromQuery]PagingParam<WorkingStyleEnum.WorkingStyleSort> paginationModel, 
        [FromQuery]SearchWorkingStyleModel searchWorkingStyleModel)
    {
        IList<GetWorkingStyleDetail> result = _workingStyleService.GetWorkingStylePage(paginationModel, searchWorkingStyleModel);
        int total = await _workingStyleService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetWorkingStyleDetail>()
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
    /// [Guest] Endpoint for get working style by ID
    /// </summary>
    /// <param name="id">An id of working style</param>
    /// <returns>List of working style</returns>
    /// <response code="200">Returns the working style</response>
    /// <response code="204">Returns if the working style is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetWorkingStyleDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWorkingStyleById(Guid id)
    {
        GetWorkingStyleDetail result = await _workingStyleService.GetWorkingStyleById(id);

        return Ok(new BaseResponse<GetWorkingStyleDetail>()
        {
            Code = StatusCodes.Status200OK,
            Data = result,
            Msg = "Send Request Successful",
        });
    }
    
    /// <summary>
    /// [Admin] Endpoint for create working style
    /// </summary>
    /// <param name="requestBody">An obj contains input info of a working style.</param>
    /// <returns>A working style within status 201 or error status.</returns>
    /// <response code="201">Returns the working style</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="ADMIN")]
    [ProducesResponseType(typeof(BaseResponse<GetWorkingStyleDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateWorkingStyle([FromBody] CreateWorkingStyleModel requestBody)
    {
        var result = await _workingStyleService.CreateWorkingStyleAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetWorkingStyleDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Admin] Endpoint for Admin edit working style.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of a working style.</param>
    /// <returns>A working style within status 200 or error status.</returns>
    /// <response code="200">Returns working style after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="ADMIN")]
    [ProducesResponseType(typeof(BaseResponse<GetWorkingStyleDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateWorkingStyleAsync(Guid id, [FromBody] UpdateWorkingStyleModel requestBody)
    {
        // try
        // {
            GetWorkingStyleDetail updateWorkingStyle = await _workingStyleService.UpdateWorkingStyleAsync(id, requestBody);

            return Ok(new BaseResponse<GetWorkingStyleDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateWorkingStyle,
                Msg = "Update Successful"
            });
        // }
        // catch (Exception e)
        // {
        //     return BadRequest(e);
        // }
            
    }
    
    /// <summary>
    /// [Admin] Endpoint for Admin Delete a working style.
    /// </summary>
    /// <param name="id">ID of working style</param>
    /// <returns>A working style within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="ADMIN")]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _workingStyleService.DeleteWorkingStyleAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}