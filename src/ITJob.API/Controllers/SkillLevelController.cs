using ITJob.Services.Enum;
using ITJob.Services.Services.SkillLevelServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.SkillLevel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/skill-levels")]
public class SkillLevelController : ControllerBase
{
    private readonly ISkillLevelService _skillLevelService;

    public SkillLevelController(ISkillLevelService skillLevelService)
    {
        _skillLevelService = skillLevelService;
    }
    /// <summary>
    /// [Guest] Endpoint for get all skill level with condition
    /// </summary>
    /// <param name="searchSkillLevelModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of skill level</returns>
    /// <response code="200">Returns the list of skill level</response>
    /// <response code="204">Returns if list of skill level is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetSkillLevelDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSkillLevel(
        [FromQuery]PagingParam<SkillLevelEnum.SkillLevelSort> paginationModel, 
        [FromQuery]SearchSkillLevelModel searchSkillLevelModel)
    {
        IList<GetSkillLevelDetail> result = _skillLevelService.GetSkillLevelPage(paginationModel, searchSkillLevelModel);
        int total = await _skillLevelService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetSkillLevelDetail>()
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
    /// [Guest] Endpoint for get skill level by ID
    /// </summary>
    /// <param name="id">An id of skill level</param>
    /// <returns>List of skill level</returns>
    /// <response code="200">Returns the skill level</response>
    /// <response code="204">Returns if the skill level is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetSkillLevelDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSkillLevelById(Guid id)
    {
        GetSkillLevelDetail result = await _skillLevelService.GetSkillLevelById(id);
   
        return Ok(new BaseResponse<GetSkillLevelDetail>()
        {
            Code = StatusCodes.Status200OK,
            Data = result,
            Msg = "Send Request Successful"
        });
    }
    
    /// <summary>
    /// [Admin] Endpoint for create skill level
    /// </summary>
    /// <param name="requestBody">An obj contains input info of an skill level.</param>
    /// <returns>A skill level within status 201 or error status.</returns>
    /// <response code="201">Returns the skill level</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="ADMIN")]
    [ProducesResponseType(typeof(BaseResponse<GetSkillLevelDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSkillLevel([FromBody] CreateSkillLevelModel requestBody)
    {
        var result = await _skillLevelService.CreateSkillLevelAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetSkillLevelDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Admin] Endpoint for Admin edit skill level.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an skill level.</param>
    /// <returns>A skill level within status 200 or error status.</returns>
    /// <response code="200">Returns skill level after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="ADMIN")]
    [ProducesResponseType(typeof(BaseResponse<GetSkillLevelDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateSkillLevelAsync(Guid id, [FromBody] UpdateSkillLevelModel requestBody)
    {
        try
        {
            GetSkillLevelDetail updateSkillLevel = await _skillLevelService.UpdateSkillLevelAsync(id,requestBody);

            return Ok(new BaseResponse<GetSkillLevelDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateSkillLevel,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    
    /// <summary>
    /// [Admin] Endpoint for Admin Delete a skill level.
    /// </summary>
    /// <param name="id">ID of skill level</param>
    /// <returns>A skill level within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="ADMIN")]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _skillLevelService.DeleteSkillLevelAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}