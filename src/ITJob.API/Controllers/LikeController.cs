using ITJob.Services.Enum;
using ITJob.Services.Services.LikeServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.Like;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/likes")]
public class LikeController : ControllerBase
{
    private readonly ILikeService _likeService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="likeService"></param>
    public LikeController(ILikeService likeService)
    {
        _likeService = likeService;
    }
    /// <summary>
    /// [Guest] Endpoint for get all like with condition
    /// </summary>
    /// <param name="searchLikeModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of like</returns>
    /// <response code="200">Returns the list of like</response>
    /// <response code="204">Returns if list of like is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetLikeDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllLike(
        [FromQuery]PagingParam<LikeEnum.LikeSort> paginationModel, 
        [FromQuery]SearchLikeModel searchLikeModel)
    {
        IList<GetLikeDetail> result = _likeService.GetLikePage(paginationModel, searchLikeModel);
        int total = await _likeService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetLikeDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get like page success!",
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
    /// [Guest] Endpoint for get all like with condition
    /// </summary>
    /// <param name="searchLikeModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of like</returns>
    /// <response code="200">Returns the list of like</response>
    /// <response code="204">Returns if list of like is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("date")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetLikeDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDateLike(
        [FromQuery]PagingParam<LikeEnum.LikeSort> paginationModel, 
        [FromQuery]SearchLikeModel searchLikeModel)
    {
        IList<GetLikeDetail> result = _likeService.GetLikeDatePage(paginationModel, searchLikeModel);
        int total = result.ToList().Count;
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetLikeDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get like page success!",
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
    /// [Guest] Endpoint for get all like with condition
    /// </summary>
    /// <param name="searchLikeModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of like</returns>
    /// <response code="200">Returns the list of like</response>
    /// <response code="204">Returns if list of like is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("company-date")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllCompanyDateLike(
        [FromQuery]PagingParam<LikeEnum.LikeSort> paginationModel, 
        [FromQuery]SearchLikeModel searchLikeModel)
    {
        int result = _likeService.GetLikeDateCompanyPage(paginationModel, searchLikeModel);
        if (result == 0)
        {
            return NoContent();
        }

        return Ok(result);
    }
    
    /// <summary>
    /// [Guest] Endpoint for get like by ID
    /// </summary>
    /// <param name="id">An id of like</param>
    /// <returns>List of like</returns>
    /// <response code="200">Returns the like</response>
    /// <response code="204">Returns if the like is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetLikeDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLikeById(Guid id)
    {
        GetLikeDetail result = await _likeService.GetLikeById(id);
        
        return Ok(new BaseResponse<GetLikeDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get like by id success!",
            Data = result
        });
    }
    
    /// <summary>
    /// [Admin] Endpoint for create like
    /// </summary>
    /// <param name="requestBody">An obj contains input info of an like.</param>
    /// <returns>A like within status 201 or error status.</returns>
    /// <response code="201">Returns the like</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetLikeDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateLike([FromBody] CreateLikeModel requestBody)
    {
        var result = await _likeService.CreateLikeAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetLikeDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Admin] Endpoint for Admin edit like.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an like.</param>
    /// <returns>A like within status 200 or error status.</returns>
    /// <response code="200">Returns like after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetLikeDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateLikeAsync(Guid id, [FromBody] UpdateLikeModel requestBody)
    {
        try
        {
            GetLikeDetail updateLike = await _likeService.UpdateLikeAsync(id, requestBody);

            return Ok(new BaseResponse<GetLikeDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateLike,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    
    /// <summary>
    /// [Admin] Endpoint for company accept like of applicant.
    /// </summary>
    /// <param name="requestBody">An obj contains update info of an like.</param>
    /// <returns>A like within status 200 or error status.</returns>
    /// <response code="200">Returns like after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("company")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetLikeDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateLikeForCompanyAsync([FromBody] UpdateMatchModel requestBody)
    {
        try
        {
            GetLikeDetail updateLike = await _likeService.CreateLikeForCompanyAsync(requestBody);

            return Ok(new BaseResponse<GetLikeDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateLike,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    /// <summary>
    /// [Admin] Endpoint for applicant accept like of company.
    /// </summary>
    /// <param name="requestBody">An obj contains update info of an like.</param>
    /// <returns>A like within status 200 or error status.</returns>
    /// <response code="200">Returns like after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("applicant")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetLikeDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateLikeForApplicantAsync([FromBody] UpdateMatchModel requestBody)
    {
        try
        {
            GetLikeDetail updateLike = await _likeService.CreateLikeForApplicantAsync(requestBody);

            return Ok(new BaseResponse<GetLikeDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateLike,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    
    /// <summary>
    /// [Admin] Endpoint for Admin Delete a like.
    /// </summary>
    /// <param name="id">ID of like</param>
    /// <returns>A like within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _likeService.DeleteLikeAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}