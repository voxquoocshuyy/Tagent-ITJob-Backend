using ITJob.Services.Enum;
using ITJob.Services.Services.BlockServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.Block;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/blocks")]
public class BlockController : ControllerBase
{
    private readonly IBlockService _blockService;

    public BlockController(IBlockService blockService)
    {
        _blockService = blockService;
    }
    /// <summary>
    /// [Guest] Endpoint for get all block with condition
    /// </summary>
    /// <param name="searchBlockModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of block</returns>
    /// <response code="200">Returns the list of block</response>
    /// <response code="204">Returns if list of block is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetBlockDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllBlock(
        [FromQuery]PagingParam<BlockEnum.BlockSort> paginationModel, 
        [FromQuery]SearchBlockModel searchBlockModel)
    {
        IList<GetBlockDetail> result = _blockService.GetBlockPage(paginationModel, searchBlockModel);
        int total = await _blockService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetBlockDetail>()
        {
            Code = StatusCodes.Status200OK,
            Data = result.ToList(),
            Msg = "Use API get block page success!",
            Paging = new PagingMetadata()
            {
                Page = paginationModel.Page,
                Size = paginationModel.PageSize,
                Total = total
            },
        });
    }
    
    /// <summary>
    /// [Guest] Endpoint for get block by ID
    /// </summary>
    /// <param name="id">An id of block</param>
    /// <returns>List of block</returns>
    /// <response code="200">Returns the block</response>
    /// <response code="204">Returns if the block is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetBlockDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBlockById(Guid id)
    {
        GetBlockDetail result = await _blockService.GetBlockById(id);
        return Ok(new BaseResponse<GetBlockDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get block by id success!",
            Data = result
        });
    }
    
    /// <summary>
    /// [Applicant] Endpoint for create block
    /// </summary>
    /// <param name="requestBody">An obj contains input info of a block.</param>
    /// <returns>A block within status 201 or error status.</returns>
    /// <response code="201">Returns the block</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    [Authorize(Roles ="APPLICANT")]
    [ProducesResponseType(typeof(BaseResponse<GetBlockDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateBlock([FromBody] CreateBlockModel requestBody)
    {
        var result = await _blockService.CreateBlockAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetBlockDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Applicant] Endpoint for edit block.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of a block.</param>
    /// <returns>A block within status 200 or error status.</returns>
    /// <response code="200">Returns block after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("{id}")]
    [Authorize(Roles ="APPLICANT")]
    [ProducesResponseType(typeof(BaseResponse<GetBlockDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateBlockAsync(Guid id, [FromBody] UpdateBlockModel requestBody)
    {
        try
        {
            GetBlockDetail updateBlock = await _blockService.UpdateBlockAsync(id,requestBody);

            return Ok(new BaseResponse<GetBlockDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateBlock,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    
    /// <summary>
    /// [Applicant] Endpoint for Admin Delete a block.
    /// </summary>
    /// <param name="id">ID of block</param>
    /// <returns>A block within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    [Authorize(Roles ="APPLICANT")]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _blockService.DeleteBlockAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}