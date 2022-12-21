using ITJob.Services.Enum;
using ITJob.Services.Services.ProjectServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/projects")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }
    /// <summary>
    /// [Guest] Endpoint for get all project with condition
    /// </summary>
    /// <param name="searchProjectModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of project</returns>
    /// <response code="200">Returns the list of project</response>
    /// <response code="204">Returns if list of project is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetProjectDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProject(
        [FromQuery]PagingParam<ProjectEnum.ProjectSort> paginationModel, 
        [FromQuery]SearchProjectModel searchProjectModel)
    {
        IList<GetProjectDetail> result = _projectService.GetProjectPage(paginationModel, searchProjectModel);
        int total = await _projectService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetProjectDetail>()
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
    /// [Guest] Endpoint for get project by ID
    /// </summary>
    /// <param name="id">An id of project</param>
    /// <returns>List of project</returns>
    /// <response code="200">Returns the project</response>
    /// <response code="204">Returns if the project is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetProjectDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProjectById(Guid id)
    {
        GetProjectDetail result = await _projectService.GetProjectById(id);

        return Ok(new BaseResponse<GetProjectDetail>()
        {
            Code = StatusCodes.Status200OK,
            Data = result,
            Msg = "Send Request Successful",
        });
    }
    
    /// <summary>
    /// [Applicant] Endpoint for create project
    /// </summary>
    /// <param name="requestBody">An obj contains input info of an project.</param>
    /// <returns>A project within status 201 or error status.</returns>
    /// <response code="201">Returns the project</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="APPLICANT")]
    [ProducesResponseType(typeof(BaseResponse<GetProjectDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectModel requestBody)
    {
        var result = await _projectService.CreateProjectAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetProjectDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Applicant] Endpoint for Admin edit project.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an project.</param>
    /// <returns>A project within status 200 or error status.</returns>
    /// <response code="200">Returns project after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="APPLICANT")]
    [ProducesResponseType(typeof(BaseResponse<GetProjectDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProjectAsync(Guid id, [FromBody] UpdateProjectModel requestBody)
    {
        try
        {
            GetProjectDetail updateProject = await _projectService.UpdateProjectAsync(id, requestBody);

            return Ok(new BaseResponse<GetProjectDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateProject,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    
    /// <summary>
    /// [Applicant] Endpoint for Admin Delete a project.
    /// </summary>
    /// <param name="id">ID of project</param>
    /// <returns>A project within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="APPLICANT")]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _projectService.DeleteProjectAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}