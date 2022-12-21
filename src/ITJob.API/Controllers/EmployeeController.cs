using ITJob.Services.Enum;
using ITJob.Services.Services.EmployeeServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/employees")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="employeeService"></param>
    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
    /// <summary>
    /// [Guest] Endpoint for get all employee with condition
    /// </summary>
    /// <param name="searchEmployeeModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of employee</returns>
    /// <response code="200">Returns the list of employee</response>
    /// <response code="204">Returns if list of employee is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetEmployeeDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEmployee(
        [FromQuery]PagingParam<EmployeeEnum.EmployeeSort> paginationModel, 
        [FromQuery]SearchEmployeeModel searchEmployeeModel)
    {
        IList<GetEmployeeDetail> result = _employeeService.GetEmployeePage(paginationModel, searchEmployeeModel);
        int total = await _employeeService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetEmployeeDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get employee page success!",
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
    /// [Guest] Endpoint for get employee by ID
    /// </summary>
    /// <param name="id">An id of employee</param>
    /// <returns>List of employee</returns>
    /// <response code="200">Returns the employee</response>
    /// <response code="204">Returns if the employee is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetEmployeeDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployeeById(Guid id)
    {
        GetEmployeeDetail result = await _employeeService.GetEmployeeById(id);
        
        return Ok(new BaseResponse<GetEmployeeDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get employee by id success!",
            Data = result
        });
    }
    
    /// <summary>
    /// [Guest] Endpoint for create employee
    /// </summary>
    /// <param name="requestBody">An obj contains input info of an employee.</param>
    /// <returns>A employee within status 201 or error status.</returns>
    /// <response code="201">Returns the employee</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetEmployeeDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeModel requestBody)
    {
        var result = await _employeeService.CreateEmployeeAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetEmployeeDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Employee] Endpoint for edit employee.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an employee.</param>
    /// <returns>A employee within status 200 or error status.</returns>
    /// <response code="200">Returns employee after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="EMPLOYEE")]
    [ProducesResponseType(typeof(BaseResponse<GetEmployeeDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateEmployeeAsync(Guid id, [FromBody] UpdateEmployeeModel requestBody)
    {
        try
        {
            GetEmployeeDetail updateEmployee = await _employeeService.UpdateEmployeeAsync(id, requestBody);

            return Ok(new BaseResponse<GetEmployeeDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateEmployee,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    /// <summary>
    /// [Employee] Endpoint for employee edit password.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="currentPassword"></param>
    /// <param name="newPassword"></param>
    /// <returns>A employee within status 200 or error status.</returns>
    /// <response code="200">Returns employee after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("password")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="EMPLOYEE")]
    [ProducesResponseType(typeof(BaseResponse<GetEmployeeDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePasswordEmployeeAsync(Guid id, string currentPassword, string newPassword)
    {
        try
        {
            string result = await _employeeService.UpdatePasswordEmployeeAsync(id, currentPassword, newPassword);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    /// <summary>
    /// [Company/Admin] Endpoint for delete a employee.
    /// </summary>
    /// <param name="id">ID of employee</param>
    /// <returns>A employee within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    [Authorize(Roles ="ADMIN")]
    [Authorize(Roles ="EMPLOYEE")]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _employeeService.DeleteEmployeeAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}