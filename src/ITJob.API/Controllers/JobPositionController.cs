using ITJob.Services.Enum;
using ITJob.Services.Services.JobPositionServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.JobPosition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/job-positions")]
public class JobPositionController : ControllerBase
{
    private readonly IJobPositionService _jobPositionService;

    public JobPositionController(IJobPositionService jobPositionService)
    {
        _jobPositionService = jobPositionService;
    }
    
    /// <summary>
    /// [Guest] Endpoint for get all job position with condition
    /// </summary>
    /// <param name="searchJobPositionModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of jobPosition</returns>
    /// <response code="200">Returns the list of job position</response>
    /// <response code="204">Returns if list of job position is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetJobPositionDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllJobPosition(
        [FromQuery]PagingParam<JobPositionEnum.JobPositionSort> paginationModel, 
        [FromQuery]SearchJobPositionModel searchJobPositionModel)
    {
        IList<GetJobPositionDetail> result = _jobPositionService.GetJobPositionPage(paginationModel, searchJobPositionModel);
        int total = await _jobPositionService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetJobPositionDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get job position page success!",
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
    /// [Guest] Endpoint for get job position by ID
    /// </summary>
    /// <param name="id">An id of job position</param>
    /// <returns>List of job position</returns>
    /// <response code="200">Returns the job position</response>
    /// <response code="204">Returns if the job position is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetJobPositionDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJobPositionById(Guid id)
    {
        GetJobPositionDetail result = await _jobPositionService.GetJobPositionById(id);

        return Ok(new BaseResponse<GetJobPositionDetail>()
        {
            Code = StatusCodes.Status200OK,
            Data = result,
            Msg = "Send Request Successful"
        });
    }
    
    /// <summary>
    /// [Admin] Endpoint for create job position
    /// </summary>
    /// <param name="requestBody">An obj contains input info of an job position.</param>
    /// <returns>A job position within status 201 or error status.</returns>
    /// <response code="201">Returns the job position</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    [Authorize(Roles ="ADMIN")]
    [ProducesResponseType(typeof(BaseResponse<GetJobPositionDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateJobPosition([FromBody] CreateJobPositionModel requestBody)
    {
        var result = await _jobPositionService.CreateJobPositionAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetJobPositionDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Admin] Endpoint for Admin edit job position.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an job position.</param>
    /// <returns>A job position within status 200 or error status.</returns>
    /// <response code="200">Returns job position after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("{id}")]
    [Authorize(Roles ="ADMIN")]
    [ProducesResponseType(typeof(BaseResponse<GetJobPositionDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateJobPositionAsync(Guid id, [FromBody] UpdateJobPositionModel requestBody)
    {
        try
        {
            GetJobPositionDetail updateJobPosition = await _jobPositionService.UpdateJobPositionAsync(id, requestBody);

            return Ok(new BaseResponse<GetJobPositionDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateJobPosition,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    
    /// <summary>
    /// [Admin] Endpoint for Admin Delete a job position.
    /// </summary>
    /// <param name="id">ID of job position</param>
    /// <returns>A job position within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    [Authorize(Roles ="ADMIN")]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _jobPositionService.DeleteJobPositionAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}