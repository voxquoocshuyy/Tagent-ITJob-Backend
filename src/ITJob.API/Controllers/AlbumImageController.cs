using ITJob.Services.Enum;
using ITJob.Services.Services.AlbumImageServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.AlbumImage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/album-images")]
public class AlbumImageController : ControllerBase
{
    private readonly IAlbumImageService _albumImageService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="albumImageService"></param>
    public AlbumImageController(IAlbumImageService albumImageService)
    {
        _albumImageService = albumImageService;
    }
    /// <summary>
    /// [Guest] Endpoint for get all album image with condition
    /// </summary>
    /// <param name="searchAlbumImageModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of album image</returns>
    /// <response code="200">Returns the list of album image</response>
    /// <response code="204">Returns if list of album image is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetAlbumImageDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAlbumImage(
        [FromQuery]PagingParam<AlbumImageEnum.AlbumImageSort> paginationModel, 
        [FromQuery]SearchAlbumImageModel searchAlbumImageModel)
    {
        try
        {
            IList<GetAlbumImageDetail> result = _albumImageService.GetAlbumImagePage(paginationModel, searchAlbumImageModel);
            int total = await _albumImageService.GetTotal();
            if (!result.Any())
            {
                return NoContent();
            }

            return Ok(new ModelsResponse<GetAlbumImageDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = result.ToList(),
                Msg = "Use API get album image page success!",
                Paging = new PagingMetadata()
                {
                    Page = paginationModel.Page,
                    Size = paginationModel.PageSize,
                    Total = total
                },
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        
    }
    
    /// <summary>
    /// [Guest] Endpoint for get album image by ID
    /// </summary>
    /// <param name="id">An id of album image</param>
    /// <returns>List of albumImage</returns>
    /// <response code="200">Returns the album image</response>
    /// <response code="204">Returns if the album image is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetAlbumImageDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAlbumImageById(Guid id)
    {
        try
        {
            GetAlbumImageDetail result = await _albumImageService.GetAlbumImageById(id);
            return Ok(new BaseResponse<GetAlbumImageDetail>()
            {
                Code = StatusCodes.Status200OK,
                Msg = "Use API get album image by id success!",
                Data = result
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
    
    /// <summary>
    /// [Admin] Endpoint for create album image
    /// </summary>
    /// <param name="requestBody">An obj contains input info of a album image.</param>
    /// <returns>A album image within status 201 or error status.</returns>
    /// <response code="201">Returns the album image</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetAlbumImageDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAlbumImage([FromForm] CreateAlbumImageModel requestBody)
    {
        try
        {
            var result = await _albumImageService.CreateAlbumImageAsync(requestBody);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        
    }
    
    /// <summary>
    /// [Admin] Endpoint for create album image
    /// </summary>
    /// <param name="requestBody">An obj contains input info of a album image.</param>
    /// <returns>A album image within status 201 or error status.</returns>
    /// <response code="201">Returns the album image</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost("url")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetAlbumImageDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAlbumImage([FromBody] CreateAlbumImageUrlModel requestBody)
    {
        try
        {
            var result = await _albumImageService.CreateAlbumImageUrlAsync(requestBody);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        
    }

    /// <summary>
    /// [Admin] Endpoint for Admin edit album image.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of a album image.</param>
    /// <returns>A album image within status 200 or error status.</returns>
    /// <response code="200">Returns album image after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetAlbumImageDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAlbumImageAsync(Guid id, [FromForm] UpdateAlbumImageModel requestBody)
    {
        try
        {
            var result = await _albumImageService.UpdateAlbumImageAsync(id,requestBody);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    
    /// <summary>
    /// [Admin] Endpoint for Admin Delete a album image.
    /// </summary>
    /// <param name="id">ID of album image</param>
    /// <returns>A album image within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    public async Task<IActionResult> DeleteAlbumImageAsync(Guid id)
    {
        try
        {
            await _albumImageService.DeleteAlbumImageAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
    /// <summary>
    /// [Admin] Endpoint for Admin Delete a album image.
    /// </summary>
    /// <param name="profileApplicantId">ID profile applicant of album image</param>
    /// <returns>A album image within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("profileApplicantId")]
    [Authorize]
    public async Task<IActionResult> DeleteAlbumImageByProfileApplicantIdAsync(Guid profileApplicantId)
    {
        try
        {
            await _albumImageService.DeleteAlbumImageByProfileApplicantIdAsync(profileApplicantId);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}