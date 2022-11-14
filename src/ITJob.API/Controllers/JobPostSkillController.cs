using ITJob.Services.Enum;
using ITJob.Services.Services.JobPostSkillServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.JobPostSkill;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/job-post-skills")]
public class JobPostSkillController : ControllerBase
{
    private readonly IJobPostSkillService _jobPostSkillService;

    public JobPostSkillController(IJobPostSkillService jobPostSkillService)
    {
        _jobPostSkillService = jobPostSkillService;
    }
    /// <summary>
    /// [Guest] Endpoint for get all job post skill with condition
    /// </summary>
    /// <param name="searchJobPostSkillModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of job post skill</returns>
    /// <response code="200">Returns the list of job post skill</response>
    /// <response code="204">Returns if list of job post skill is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetJobPostSkillDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllJobPostSkill(
        [FromQuery]PagingParam<JobPostSkillEnum.JobPostSkillSort> paginationModel, 
        [FromQuery]SearchJobPostSkillModel searchJobPostSkillModel)
    {
        IList<GetJobPostSkillDetail> result = _jobPostSkillService.GetJobPostSkillPage(paginationModel, searchJobPostSkillModel);
        int total = await _jobPostSkillService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetJobPostSkillDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get jobPostSkill page success!",
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
    /// [Guest] Endpoint for get job post skill by ID
    /// </summary>
    /// <param name="id">An id of job post skill</param>
    /// <returns>List of job post skill</returns>
    /// <response code="200">Returns the job post skill</response>
    /// <response code="204">Returns if the job post skill is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetJobPostSkillDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJobPostSkillById(Guid id)
    {
        GetJobPostSkillDetail result = await _jobPostSkillService.GetJobPostSkillById(id);
        
        return Ok(new BaseResponse<GetJobPostSkillDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get jobPostSkill by id success!",
            Data = result
        });
    }
    
    /// <summary>
    /// [Admin] Endpoint for create job post skill
    /// </summary>
    /// <param name="requestBody">An obj contains input info of an job post skill.</param>
    /// <returns>A job post skill within status 201 or error status.</returns>
    /// <response code="201">Returns the job post skill</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetJobPostSkillDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateJobPostSkill([FromBody] CreateJobPostSkillModel requestBody)
    {
        var result = await _jobPostSkillService.CreateJobPostSkillAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetJobPostSkillDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Admin] Endpoint for Admin edit job post skill.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an job post skill.</param>
    /// <returns>A job post skill within status 200 or error status.</returns>
    /// <response code="200">Returns job post skill after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetJobPostSkillDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateJobPostSkillAsync(Guid id, [FromBody] UpdateJobPostSkillModel requestBody)
    {
        try
        {
            GetJobPostSkillDetail updateJobPostSkill = await _jobPostSkillService.UpdateJobPostSkillAsync(id, requestBody);

            return Ok(new BaseResponse<GetJobPostSkillDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateJobPostSkill,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    
    /// <summary>
    /// [Admin] Endpoint for Admin Delete a job post skill.
    /// </summary>
    /// <param name="id">ID of job post skill</param>
    /// <returns>A job post skill within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _jobPostSkillService.DeleteJobPostSkillAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}