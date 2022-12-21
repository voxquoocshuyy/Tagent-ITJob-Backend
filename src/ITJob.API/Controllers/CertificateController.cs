using ITJob.Services.Enum;
using ITJob.Services.Services.CertificateServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/certificates")]
public class CertificateController : ControllerBase
{
    private readonly ICertificateService _certificateService;

    public CertificateController(ICertificateService certificateService)
    {
        _certificateService = certificateService;
    }
    /// <summary>
    /// [Guest] Endpoint for get all certificate with condition
    /// </summary>
    /// <param name="searchCertificateModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of certificate</returns>
    /// <response code="200">Returns the list of certificate</response>
    /// <response code="204">Returns if list of certificate is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetCertificateDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCertificate(
        [FromQuery]PagingParam<CertificateEnum.CertificateSort> paginationModel, 
        [FromQuery]SearchCertificateModel searchCertificateModel)
    {
        IList<GetCertificateDetail> result = _certificateService.GetCertificatePage(paginationModel, searchCertificateModel);
        int total = await _certificateService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetCertificateDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get certificate page success!",
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
    /// [Guest] Endpoint for get certificate by ID
    /// </summary>
    /// <param name="id">An id of certificate</param>
    /// <returns>List of certificate</returns>
    /// <response code="200">Returns the certificate</response>
    /// <response code="204">Returns if the certificate is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetCertificateDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCertificateById(Guid id)
    {
        GetCertificateDetail result = await _certificateService.GetCertificateById(id);
        
        return Ok(new BaseResponse<GetCertificateDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get certificate by id success!",
            Data = result
        });
    }
    
    /// <summary>
    /// [Applicant] Endpoint for create certificate
    /// </summary>
    /// <param name="requestBody">An obj contains input info of an certificate.</param>
    /// <returns>A certificate within status 201 or error status.</returns>
    /// <response code="201">Returns the certificate</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="APPLICANT")]
    [ProducesResponseType(typeof(BaseResponse<GetCertificateDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCertificate([FromBody] CreateCertificateModel requestBody)
    {
        var result = await _certificateService.CreateCertificateAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetCertificateDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Applicant] Endpoint for edit certificate.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an certificate.</param>
    /// <returns>A certificate within status 200 or error status.</returns>
    /// <response code="200">Returns certificate after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="APPLICANT")]
    [ProducesResponseType(typeof(BaseResponse<GetCertificateDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateCertificateAsync(Guid id, [FromBody] UpdateCertificateModel requestBody)
    {
        try
        {
            GetCertificateDetail updateCertificate = await _certificateService.UpdateCertificateAsync(id, requestBody);

            return Ok(new BaseResponse<GetCertificateDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateCertificate,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    
    /// <summary>
    /// [Applicant] Endpoint for delete a certificate.
    /// </summary>
    /// <param name="id">ID of certificate</param>
    /// <returns>A certificate within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [Authorize(Roles ="APPLICANT")]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _certificateService.DeleteCertificateAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}