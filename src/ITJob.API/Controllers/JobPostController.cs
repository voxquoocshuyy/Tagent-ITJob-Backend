using ITJob.Services.Enum;
using ITJob.Services.Services.JobPostServices;
using ITJob.Services.Services.MatchServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.JobPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/job-posts")]
public class JobPostController : ControllerBase
{
    private readonly IJobPostService _jobPostService;
    private readonly IMatchService _matchService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="jobPostService"></param>
    /// <param name="matchService"></param>
    public JobPostController(IJobPostService jobPostService, IMatchService matchService)
    {
        _jobPostService = jobPostService;
        _matchService = matchService;
    }
    /// <summary>
    /// [Guest] Endpoint for get all job post with condition
    /// </summary>
    /// <param name="searchJobPostModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of job post</returns>
    /// <response code="200">Returns the list of job post</response>
    /// <response code="204">Returns if list of job post is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetJobPostDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllJobPost(
        [FromQuery]PagingParam<JobPostEnum.JobPostSort> paginationModel, 
        [FromQuery]SearchJobPostModel searchJobPostModel)
    {
        IList<GetJobPostDetail> result = _jobPostService.GetJobPostPage(paginationModel, searchJobPostModel);
        int total = await _jobPostService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }
        return Ok(new ModelsResponse<GetJobPostDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get jobPost page success!",
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
    /// [Guest] Endpoint for get all job post like profile applicant with condition
    /// </summary>
    /// <param name="searchJobPostModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <param name="profileApplicantId"></param>
    /// <returns>List of job post</returns>
    /// <response code="200">Returns the list of job post</response>
    /// <response code="204">Returns if list of job post is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("like")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetJobPostDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJobPostLike(
        [FromQuery]PagingParam<JobPostEnum.JobPostSort> paginationModel, 
        [FromQuery]SearchJobPostModel searchJobPostModel, Guid profileApplicantId)
    {
        IList<GetJobPostDetail> result = _jobPostService.GetJobPostLikePage(paginationModel, searchJobPostModel, profileApplicantId);
        int total = await _jobPostService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetJobPostDetail>()
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
    /// [Guest] Endpoint for get all job post like profile applicant with condition
    /// </summary>
    /// <param name="searchJobPostModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <param name="profileApplicantId"></param>
    /// <returns>List of job post</returns>
    /// <response code="200">Returns the list of job post</response>
    /// <response code="204">Returns if list of job post is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("profileApplicant-like")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetJobPostDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJobPostProfileApplicantLikePage(Guid profileApplicantId,
        [FromQuery]PagingParam<JobPostEnum.JobPostSort> paginationModel, 
        [FromQuery]SearchJobPostModel searchJobPostModel)
    {
        var result = _jobPostService.
            GetJobPostProfileApplicantLikePage( paginationModel, searchJobPostModel, profileApplicantId);

        return Ok(new ModelsResponse<GetJobPostDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get job post sort by system success!",
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
    /// [Guest] Endpoint for get all job post with sort by system
    /// </summary>
    /// <param name="profileApplicantId"> an id of profile applicant</param>
    /// <param name="paginationModel"></param>
    /// <param name="searchJobPostModel"></param>
    /// <returns>List of job post</returns>
    /// <response code="200">Returns the list of job post</response>
    /// <response code="204">Returns if list of job post is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("profileApplicantId")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetJobPostDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllJobPostSortBySystem(
        [FromQuery]PagingParam<JobPostEnum.JobPostSort> paginationModel, 
        [FromQuery]SearchJobPostModel searchJobPostModel, Guid profileApplicantId)
    {
        var result = await _matchService.
            CalculatorTotalScoreForProfileApplicantFilter(profileApplicantId, paginationModel, searchJobPostModel);
        return Ok(new ModelsResponse<GetJobPostDetail>
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get job post sort by system success!",
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
    /// [Guest] Endpoint for get job post by ID
    /// </summary>
    /// <param name="id">An id of job post</param>
    /// <returns>List of job post</returns>
    /// <response code="200">Returns the job post</response>
    /// <response code="204">Returns if the job post is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetJobPostDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJobPostById(Guid id)
    {
        GetJobPostDetail result = await _jobPostService.GetJobPostById(id);
        
        return Ok(new BaseResponse<GetJobPostDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get jobPost by id success!",
            Data = result
        });
    }
    
    /// <summary>
    /// [Employee] Endpoint for create job post
    /// </summary>
    /// <param name="requestBody">An obj contains input info of an job post.</param>
    /// <returns>A job post within status 201 or error status.</returns>
    /// <response code="201">Returns the job post</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="EMPLOYEE")]
    [ProducesResponseType(typeof(BaseResponse<GetJobPostDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateJobPost([FromBody] CreateJobPostModel requestBody)
    {
        var result = await _jobPostService.CreateJobPostAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetJobPostDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Employee] Endpoint for edit job post.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an job post.</param>
    /// <returns>A job post within status 200 or error status.</returns>
    /// <response code="200">Returns job post after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="EMPLOYEE")]
    [ProducesResponseType(typeof(BaseResponse<GetJobPostDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateJobPostAsync(Guid id, [FromBody] UpdateJobPostModel requestBody)
    {
        try
        {
            GetJobPostDetail updateJobPost = await _jobPostService.UpdateJobPostAsync(id, requestBody);

            return Ok(new BaseResponse<GetJobPostDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateJobPost,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    /// <summary>
    /// [Employee] Endpoint for edit job post.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an job post.</param>
    /// <returns>A job post within status 200 or error status.</returns>
    /// <response code="200">Returns job post after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("expired/{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="EMPLOYEE")]
    [ProducesResponseType(typeof(BaseResponse<GetJobPostDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateJobPostExpiredAsync(Guid id, [FromBody] UpdateJobPostExpriredModel requestBody)
    {
        try
        {
            GetJobPostDetail updateJobPost = await _jobPostService.UpdateJobPostExpiredAsync(id, requestBody);

            return Ok(new BaseResponse<GetJobPostDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateJobPost,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    /// <summary>
    /// [Employee] Endpoint for employee edit job post.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an job post.</param>
    /// <returns>A job post within status 200 or error status.</returns>
    /// <response code="200">Returns job post after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("money/{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="EMPLOYEE")]
    [ProducesResponseType(typeof(BaseResponse<GetJobPostDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateJobPostMoneyAsync(Guid id, [FromBody] UpdateJobPostMoneyModel requestBody)
    {
        try
        {
            GetJobPostDetail updateJobPost = await _jobPostService.UpdateJobPostMoneyAsync(id, requestBody);

            return Ok(new BaseResponse<GetJobPostDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateJobPost,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    /// <summary>
    /// [Company] Endpoint for company approval job post.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains approval a job post.</param>
    /// <returns>A job post within status 200 or error status.</returns>
    /// <response code="200">Returns job post after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("approval")]
    [Authorize(Roles ="COMPANY")]
    [ProducesResponseType(typeof(BaseResponse<GetJobPostDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ApprovalJobPostAsync(Guid id, [FromBody] ApprovalJobPostModel requestBody)
    {
        try
        {
            GetJobPostDetail updateJobPost = await _jobPostService.ApprovalJobPostAsync(id, requestBody);

            return Ok(new BaseResponse<GetJobPostDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateJobPost,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    
    /// <summary>
    /// [Admin] Endpoint for Admin Delete a job post.
    /// </summary>
    /// <param name="id">ID of job post</param>
    /// <returns>A job post within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    [Authorize(Roles ="ADMIN")]
    public async Task<IActionResult> DeleteJobPostAsync(Guid id)
    {
        try
        {
            await _jobPostService.DeleteJobPostAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}