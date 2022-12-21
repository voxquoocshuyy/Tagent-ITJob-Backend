using ITJob.Services.Enum;
using ITJob.Services.Services.SkillGroupServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.SkillGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/skill-groups")]
public class SkillGroupController : ControllerBase
{
    private readonly ISkillGroupService _skillGroupService;

    public SkillGroupController(ISkillGroupService skillGroupService)
    {
        _skillGroupService = skillGroupService;
    }
    
    /// <summary>
    /// [Guest] Endpoint for get all skill group with condition
    /// </summary>
    /// <param name="searchSkillGroupModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of skill group</returns>
    /// <response code="200">Returns the list of skill group</response>
    /// <response code="204">Returns if list of skill group is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetSkillGroupDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSkillGroup(
        [FromQuery]PagingParam<SkillGroupEnum.SkillGroupSort> paginationModel, 
        [FromQuery]SearchSkillGroupModel searchSkillGroupModel)
    {
        IList<GetSkillGroupDetail> result = _skillGroupService.GetSkillGroupPage(paginationModel, searchSkillGroupModel);
        int total = await _skillGroupService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetSkillGroupDetail>()
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
    /// [Guest] Endpoint for get skill group by ID
    /// </summary>
    /// <param name="id">An id of skill group</param>
    /// <returns>List of skill group</returns>
    /// <response code="200">Returns the skill group</response>
    /// <response code="204">Returns if the skill group is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetSkillGroupDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSkillGroupById(Guid id)
    {
        GetSkillGroupDetail result = await _skillGroupService.GetSkillGroupById(id);
            
        if (result == null)
        {
            return NoContent();
        }
            
        return Ok(new BaseResponse<GetSkillGroupDetail>()
        {
            Code = StatusCodes.Status200OK,
            Data = result,
            Msg = "Send Request Successful",
        });
    }
    
    /// <summary>
    /// [Admin] Endpoint for create skill group
    /// </summary>
    /// <param name="requestBody">An obj contains input info of an skill group.</param>
    /// <returns>A skill group within status 201 or error status.</returns>
    /// <response code="201">Returns the skill group</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="ADMIN")]
    [ProducesResponseType(typeof(BaseResponse<GetSkillGroupDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSkillGroup([FromBody] CreateSkillGroupModel requestBody)
    {
        var result = await _skillGroupService.CreateSkillGroupAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetSkillGroupDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Admin] Endpoint for Admin edit skill group.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an skill group.</param>
    /// <returns>A skill group within status 200 or error status.</returns>
    /// <response code="200">Returns skill group after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="ADMIN")]
    [ProducesResponseType(typeof(BaseResponse<GetSkillGroupDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateSkillGroupAsync(Guid id, [FromBody] UpdateSkillGroupModel requestBody)
    {
        try
        {
            GetSkillGroupDetail updateSkillGroup = await _skillGroupService.UpdateSkillGroupAsync(id, requestBody);

            return Ok(new BaseResponse<GetSkillGroupDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateSkillGroup,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    
    /// <summary>
    /// [Admin] Endpoint for Admin Delete a skill group.
    /// </summary>
    /// <param name="id">ID of skill group</param>
    /// <returns>A skill group within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="ADMIN")]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _skillGroupService.DeleteSkillGroupAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}