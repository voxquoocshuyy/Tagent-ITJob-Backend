using ITJob.Services.Enum;
using ITJob.Services.Services.RoleServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/roles")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }
    /// <summary>
    /// [Guest] Endpoint for get all role with condition
    /// </summary>
    /// <param name="searchRoleModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of role</returns>
    /// <response code="200">Returns the list of role</response>
    /// <response code="204">Returns if list of role is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetRoleDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRole(
        [FromQuery]PagingParam<RoleEnum.RoleSort> paginationModel, 
        [FromQuery]SearchRoleModel searchRoleModel)
    {
        IList<GetRoleDetail> result = _roleService.GetRolePage(paginationModel, searchRoleModel);
        int total = await _roleService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetRoleDetail>()
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
    /// [Guest] Endpoint for get role by ID
    /// </summary>
    /// <param name="id">An id of role</param>
    /// <returns>List of role</returns>
    /// <response code="200">Returns the role</response>
    /// <response code="204">Returns if the role is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetRoleDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoleById(Guid id)
    {
        GetRoleDetail result = await _roleService.GetRoleById(id);
   
        return Ok(new BaseResponse<GetRoleDetail>()
        {
            Code = StatusCodes.Status200OK,
            Data = result,
            Msg = "Send Request Successful"
        });
    }
    
    /// <summary>
    /// [Admin] Endpoint for create role
    /// </summary>
    /// <param name="requestBody">An obj contains input info of a role.</param>
    /// <returns>A role within status 201 or error status.</returns>
    /// <response code="201">Returns the role</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetRoleDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleModel requestBody)
    {
        var result = await _roleService.CreateRoleAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetRoleDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }
    
    /// <summary>
    /// [Admin] Endpoint for Admin edit role.
    /// </summary>
    /// <param name="requestBody">An obj contains update info of a role.</param>
    /// <returns>A role within status 200 or error status.</returns>
    /// <response code="200">Returns role after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetRoleDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateRoleAsync(Guid id, [FromBody] UpdateRoleModel requestBody)
    {
            GetRoleDetail updateRole = await _roleService.UpdateRoleAsync(id, requestBody);

            return Ok(new BaseResponse<GetRoleDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateRole,
                Msg = "Update Successful"
            });
   
    }
    
    /// <summary>
    /// [Admin] Endpoint for Admin Delete a role.
    /// </summary>
    /// <param name="id">ID of role</param>
    /// <returns>A role within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _roleService.DeleteRoleAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}