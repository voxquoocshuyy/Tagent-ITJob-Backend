using ITJob.Services.Enum;
using ITJob.Services.Services.SkillServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.Skill;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/skills")]
public class SkillController : ControllerBase
{
    private readonly ISkillService _skillService;

    public SkillController(ISkillService skillService)
    {
        _skillService = skillService;
    }
    /// <summary>
    /// [Guest] Endpoint for get all skill with condition
    /// </summary>
    /// <param name="searchSkillModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of skill</returns>
    /// <response code="200">Returns the list of skill</response>
    /// <response code="204">Returns if list of skill is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetSkillDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSkill(
        [FromQuery]PagingParam<SkillEnum.SkillSort> paginationModel, 
        [FromQuery]SearchSkillModel searchSkillModel)
    {
        IList<GetSkillDetail> result = _skillService.GetSkillPage(paginationModel, searchSkillModel);
        int total = await _skillService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetSkillDetail>()
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
    /// [Guest] Endpoint for get skill by ID
    /// </summary>
    /// <param name="id">An id of skill</param>
    /// <returns>List of skill</returns>
    /// <response code="200">Returns the skill</response>
    /// <response code="204">Returns if the skill is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetSkillDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSkillById(Guid id)
    {
        GetSkillDetail result = await _skillService.GetSkillById(id);

        return Ok(new BaseResponse<GetSkillDetail>()
        {
            Code = StatusCodes.Status200OK,
            Data = result,
            Msg = "Send Request Successful"
        });
    }
    
    /// <summary>
    /// [Admin] Endpoint for create skill
    /// </summary>
    /// <param name="requestBody">An obj contains input info of a skill.</param>
    /// <returns>A skill within status 201 or error status.</returns>
    /// <response code="201">Returns the skill</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    [Authorize(Roles ="ADMIN")]
    [ProducesResponseType(typeof(BaseResponse<GetSkillDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSkill([FromBody] CreateSkillModel requestBody)
    {
        var result = await _skillService.CreateSkillAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetSkillDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Admin] Endpoint for Admin edit skill.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of a skill.</param>
    /// <returns>A skill within status 200 or error status.</returns>
    /// <response code="200">Returns skill after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut]
    // [Authorize(Skills = SkillsConstants.ADMIN)]
    [Authorize(Roles ="ADMIN")]
    [ProducesResponseType(typeof(BaseResponse<GetSkillDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateSkillAsync(Guid id, [FromBody] UpdateSkillModel requestBody)
    {
        // try
        // {
            GetSkillDetail updateSkill = await _skillService.UpdateSkillAsync(id, requestBody);

            return Ok(new BaseResponse<GetSkillDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateSkill,
                Msg = "Update Successful"
            });
        // }
        // catch (Exception e)
        // {
        //     return BadRequest(e);
        // }
            
    }
    
    /// <summary>
    /// [Admin] Endpoint for Admin Delete a skill.
    /// </summary>
    /// <param name="id">ID of skill</param>
    /// <returns>A skill within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    // [Authorize(Skills = SkillsConstants.ADMIN)]
    [Authorize(Roles ="ADMIN")]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _skillService.DeleteSkillAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}