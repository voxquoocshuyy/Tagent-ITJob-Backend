using ITJob.Services.Enum;
using ITJob.Services.Services.CompanyServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.Company;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/companies")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="companyService"></param>
    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }
    
    /// <summary>
    /// [Guest] Endpoint for get all company with condition
    /// </summary>
    /// <param name="searchCompanyModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of company</returns>
    /// <response code="200">Returns the list of company</response>
    /// <response code="204">Returns if list of company is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetCompanyDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCompany(
        [FromQuery]PagingParam<CompanyEnum.CompanySort> paginationModel, 
        [FromQuery]SearchCompanyModel searchCompanyModel)
    {
        IList<GetCompanyDetail> result = _companyService.GetCompanyPage(paginationModel, searchCompanyModel);
        int total = await _companyService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetCompanyDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get company page success!",
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
    /// [Guest] Endpoint for get company by ID
    /// </summary>
    /// <param name="id">An id of company</param>
    /// <returns>List of company</returns>
    /// <response code="200">Returns the company</response>
    /// <response code="204">Returns if the company is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetCompanyDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCompanyById(Guid id)
    {
        GetCompanyDetail result = await _companyService.GetCompanyById(id);
        
        return Ok(new BaseResponse<GetCompanyDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get company page success!",
            Data = result
        });
    }

    /// <summary>
    /// [Guest] Endpoint for get company by email
    /// </summary>
    /// <param name="email">An email of company</param>
    /// <returns>The company</returns>
    /// <response code="200">Returns the company</response>
    /// <response code="204">Returns if the company is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("email")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetCompanyDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCompanyByEmail(string email)
    {
        GetCompanyDetail result = await _companyService.GetCompanyByEmail(email);
        
        return Ok(new BaseResponse<GetCompanyDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get company page success!",
            Data = result
        });
    }
    
    /// <summary>
    /// [Guest] Endpoint for create company
    /// </summary>
    /// <param name="requestBody">An obj contains input info of an company.</param>
    /// <returns>A company within status 201 or error status.</returns>
    /// <response code="201">Returns the company</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetCompanyDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCompany([FromForm] CreateCompanyModel requestBody)
    {
        var result = await _companyService.CreateCompanyAsync(requestBody);
        
        return Created(string.Empty, new BaseResponse<GetCompanyDetail>()
        {
            Code = StatusCodes.Status201Created,
            Msg = "Send Request Successful",
            Data = result
        });
    }

    /// <summary>
    /// [Company] Endpoint for edit company.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an company.</param>
    /// <returns>A company within status 200 or error status.</returns>
    /// <response code="200">Returns company after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="COMPANY")]
    [ProducesResponseType(typeof(BaseResponse<GetCompanyDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateCompanyAsync(Guid id, [FromForm] UpdateCompanyModel requestBody)
    {
        try
        {
            GetCompanyDetail updateCompany = await _companyService.UpdateCompanyAsync(id, requestBody);

            return Ok(new BaseResponse<GetCompanyDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateCompany,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    /// <summary>
    /// [Company] Endpoint for update company.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an company.</param>
    /// <returns>A company within status 200 or error status.</returns>
    /// <response code="200">Returns company after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("upgrade")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="COMPANY")]
    [ProducesResponseType(typeof(BaseResponse<GetCompanyDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpgradeCompanyAsync(Guid id, [FromBody] UpgradeCompanyModel requestBody)
    {
        try
        {
            GetCompanyDetail updateCompany = await _companyService.UpgradeCompanyAsync(id, requestBody);

            return Ok(new BaseResponse<GetCompanyDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateCompany,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }

    /// <summary>
    /// [Admin] Endpoint for Admin Delete a company.
    /// </summary>
    /// <param name="id">ID of company</param>
    /// <param name="updateReason"></param>
    /// <returns>A company within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="ADMIN")]
    public async Task<IActionResult> DeleteClassAsync(Guid id, UpdateReason updateReason)
    {
        try
        {
            await _companyService.DeleteCompanyAsync(id, updateReason);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
    
    /// <summary>
    /// [Company] Endpoint for company edit password.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="currentPassword"></param>
    /// <param name="newPassword"></param>
    /// <returns>A applicant within status 200 or error status.</returns>
    /// <response code="200">Returns applicant after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("password")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="COMPANY")]
    [ProducesResponseType(typeof(BaseResponse<GetCompanyDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdatePasswordApplicantAsync(Guid id, string currentPassword, string newPassword)
    {
        try
        {
            string result = await _companyService.UpdatePasswordCompanyAsync(id, currentPassword, newPassword);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    /// <summary>
    /// [Company] Endpoint for company reset password.
    /// </summary>
    /// <param name="otp"></param>
    /// <param name="newPassword"></param>
    /// <param name="email"></param>
    /// <returns>A user within status 200 or error status.</returns>
    /// <response code="200">Returns user after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("reset")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="COMPANY")]
    [ProducesResponseType(typeof(BaseResponse<GetCompanyDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgetPasswordUserAsync(string email, int otp, string newPassword)
    {
        try
        {
            string result = await _companyService.ForgetPasswordCompanyAsync(email, otp, newPassword);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}