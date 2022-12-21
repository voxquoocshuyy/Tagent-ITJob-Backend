using ITJob.Services.Enum;
using ITJob.Services.Services.TransactionServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.SystemWallet;
using ITJob.Services.ViewModels.Transaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/transactions")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transactionService"></param>
    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }
    /// <summary>
    /// [Guest] Endpoint for get all transaction with condition
    /// </summary>
    /// <param name="searchTransactionModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of transaction</returns>
    /// <response code="200">Returns the list of transaction</response>
    /// <response code="204">Returns if list of transaction is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetTransactionDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTransaction(
        [FromQuery]PagingParam<TransactionEnum.TransactionSort> paginationModel, 
        [FromQuery]SearchTransactionModel searchTransactionModel)
    {
        IList<GetTransactionDetail> result = _transactionService.GetTransactionPage(paginationModel, searchTransactionModel);
        int total = await _transactionService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetTransactionDetail>()
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
    /// [Guest] Endpoint for get transaction by ID
    /// </summary>
    /// <param name="id">An id of transaction</param>
    /// <returns>List of transaction</returns>
    /// <response code="200">Returns the transaction</response>
    /// <response code="204">Returns if the transaction is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetTransactionDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransactionById(Guid id)
    {
        GetTransactionDetail result = await _transactionService.GetTransactionById(id);

        return Ok(new BaseResponse<GetTransactionDetail>()
        {
            Code = StatusCodes.Status200OK,
            Data = result,
            Msg = "Send Request Successful",
        });
    }
    /// <summary>
    /// [Guest] Endpoint for get total of system wallet
    /// </summary>
    /// <returns>List of transaction</returns>
    /// <response code="200">Returns the transaction</response>
    /// <response code="204">Returns if the transaction is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("system")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetSystemWalletDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSystemWalletDetail()
    {
        GetSystemWalletDetail result = await _transactionService.GetSystemWallet();

        return Ok(new BaseResponse<GetSystemWalletDetail>()
        {
            Code = StatusCodes.Status200OK,
            Data = result,
            Msg = "Send Request Successful",
        });
    }
    /// <summary>
    /// [Admin] Endpoint for create transaction
    /// </summary>
    /// <param name="requestBody">An obj contains input info of a transaction.</param>
    /// <returns>A transaction within status 201 or error status.</returns>
    /// <response code="201">Returns the transaction</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetTransactionDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionModel requestBody)
    {
        var result = await _transactionService.CreateTransactionAsync(requestBody);
    
        return Created(string.Empty, new BaseResponse<GetTransactionDetail>()
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
    // [ProducesResponseType(typeof(BaseResponse<GetTransactionDetail>), StatusCodes.Status200OK)]
    // public async Task<IActionResult> UpdateTransactionAsync(Guid id, [FromBody] UpdateTransactionModel requestBody)
    // {
    //     // try
    //     // {
    //         GetTransactionDetail updateTransaction = await _transactionService.UpdateTransactionAsync(id, requestBody);
    //
    //         return Ok(new BaseResponse<GetTransactionDetail>()
    //         {
    //             Code = StatusCodes.Status200OK,
    //             Data = updateTransaction,
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
    //         await _transactionService.DeleteTransactionAsync(id);
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest(e);
    //     }
    //     return NoContent();
    // }
}