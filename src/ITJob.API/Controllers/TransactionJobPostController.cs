using ITJob.Services.Enum;
using ITJob.Services.Services.TransactionJobPostServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.SystemWallet;
using ITJob.Services.ViewModels.Transaction;
using ITJob.Services.ViewModels.TransactionJobPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/transaction-jobposts")]
public class TransactionJobPostController : ControllerBase
{
    private readonly ITransactionJobPostService _transactionJobPostService;

    public TransactionJobPostController(ITransactionJobPostService transactionJobPostService)
    {
        _transactionJobPostService = transactionJobPostService;
    }

    /// <summary>
    /// [Guest] Endpoint for get all transaction job post with condition
    /// </summary>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <param name="searchTransactionJobPostModel"></param>
    /// <returns>List of transaction</returns>
    /// <response code="200">Returns the list of transaction job post</response>
    /// <response code="204">Returns if list of transaction job post is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetTransactionJobPostDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTransactionJobPost(
        [FromQuery]PagingParam<TransactionJobPostEnum.TransactionJobPostSort> paginationModel, 
        [FromQuery]SearchTransactionJobPostModel searchTransactionJobPostModel)
    {
        IList<GetTransactionJobPostDetail> result = _transactionJobPostService.GetTransactionJobPostPage(paginationModel, searchTransactionJobPostModel);
        int total = await _transactionJobPostService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetTransactionJobPostDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Send Request Successful",
            Data = result.ToList(),
            Paging = new PagingMetadata
            {
                Page = paginationModel.Page,
                Size = paginationModel.PageSize,
                Total = total
            },
        });
    }
    
    /// <summary>
    /// [Guest] Endpoint for get transaction job post by ID
    /// </summary>
    /// <param name="id">An id of transaction</param>
    /// <returns>List of transaction</returns>
    /// <response code="200">Returns the transaction job post</response>
    /// <response code="204">Returns if the transaction job post is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetTransactionJobPostDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransactionJobPostById(Guid id)
    {
        GetTransactionJobPostDetail result = await _transactionJobPostService.GetTransactionJobPostById(id);

        return Ok(new BaseResponse<GetTransactionJobPostDetail>()
        {
            Code = StatusCodes.Status200OK,
            Data = result,
            Msg = "Send Request Successful",
        });
    }
    /// <summary>
    /// [Admin] Endpoint for create transaction job post
    /// </summary>
    /// <param name="requestBody">An obj contains input info of a transaction job post.</param>
    /// <returns>A transaction job post within status 201 or error status.</returns>
    /// <response code="201">Returns the transaction job post</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetTransactionJobPostDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTransactionJobPost([FromBody] CreateTransactionJobPostModel requestBody)
    {
        var result = await _transactionJobPostService.CreateTransactionJobPostAsync(requestBody);
    
        return Created(string.Empty, new BaseResponse<GetTransactionJobPostDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    // /// <summary>
    // /// [Admin] Endpoint for Admin edit transaction.
    // /// </summary>
    // /// <param name="id"></param>
    // /// <param name="requestBody">An obj contains update info of a transaction.</param>
    // /// <returns>A transaction within status 200 or error status.</returns>
    // /// <response code="200">Returns transaction after update</response>
    // /// <response code="403">Return if token is access denied</response>
    // [HttpPut]
    // // [Authorize(Roles = RolesConstants.ADMIN)]
    // [ProducesResponseType(typeof(BaseResponse<GetTransactionJobPostDetail>), StatusCodes.Status200OK)]
    // public async Task<IActionResult> UpdateTransactionJobPostAsync(Guid id, [FromBody] UpdateTransactionJobPostModel requestBody)
    // {
    //     // try
    //     // {
    //         GetTransactionJobPostDetail updateTransactionJobPost = await _transactionJobPostService.UpdateTransactionJobPostAsync(id, requestBody);
    //
    //         return Ok(new BaseResponse<GetTransactionJobPostDetail>()
    //         {
    //             Code = StatusCodes.Status200OK,
    //             Data = updateTransactionJobPost,
    //             Msg = "Update Successful"
    //         });
    //     // }
    //     // catch (Exception e)
    //     // {
    //     //     return BadRequest(e);
    //     // }
    //         
    // }
    //
    // /// <summary>
    // /// [Admin] Endpoint for Admin Delete a transaction.
    // /// </summary>
    // /// <param name="id">ID of transaction</param>
    // /// <returns>A transaction within status 200 or 204 status.</returns>
    // /// <response code="200">Returns 200 status</response>
    // /// <response code="204">Returns NoContent status</response>
    // [HttpDelete("{id}")]
    // // [Authorize(Roles = RolesConstants.ADMIN)]
    // public async Task<IActionResult> DeleteClassAsync(Guid id)
    // {
    //     try
    //     {
    //         await _transactionJobPostService.DeleteTransactionJobPostAsync(id);
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest(e);
    //     }
    //     return NoContent();
    // }
}