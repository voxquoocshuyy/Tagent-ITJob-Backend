using ITJob.Services.Enum;
using ITJob.Services.Services.WalletServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.Wallet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/wallets")]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="walletService"></param>
    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }
    /// <summary>
    /// [Guest] Endpoint for get all wallet with condition
    /// </summary>
    /// <param name="searchWalletModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of wallet</returns>
    /// <response code="200">Returns the list of wallet</response>
    /// <response code="204">Returns if list of wallet is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetWalletDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllWallet(
        [FromQuery]PagingParam<WalletEnum.WalletSort> paginationModel, 
        [FromQuery]SearchWalletModel searchWalletModel)
    {
        IList<GetWalletDetail> result = _walletService.GetWalletPage(paginationModel, searchWalletModel);
        int total = await _walletService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetWalletDetail>()
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
    /// [Guest] Endpoint for get wallet by ID
    /// </summary>
    /// <param name="id">An id of wallet</param>
    /// <returns>List of wallet</returns>
    /// <response code="200">Returns the wallet</response>
    /// <response code="204">Returns if the wallet is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetWalletDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWalletById(Guid id)
    {
        GetWalletDetail result = await _walletService.GetWalletById(id);

        return Ok(new BaseResponse<GetWalletDetail>()
        {
            Code = StatusCodes.Status200OK,
            Data = result,
            Msg = "Send Request Successful",
        });
    }
    
    // /// <summary>
    // /// [Admin] Endpoint for create wallet
    // /// </summary>
    // /// <param name="requestBody">An obj contains input info of a wallet.</param>
    // /// <returns>A wallet within status 201 or error status.</returns>
    // /// <response code="201">Returns the wallet</response>
    // /// <response code="403">Return if token is access denied</response>
    // [HttpPost]
    // // [Authorize(Roles = RolesConstants.ADMIN)]
    // [ProducesResponseType(typeof(BaseResponse<GetWalletDetail>), StatusCodes.Status201Created)]
    // public async Task<IActionResult> CreateWallet([FromBody] CreateWalletModel requestBody)
    // {
    //     var result = await _walletService.CreateWalletAsync(requestBody);
    //
    //     return Created(string.Empty, new BaseResponse<GetWalletDetail>()
    //     {
    //         Code = StatusCodes.Status201Created,
    //         Data = result,
    //         Msg = "Send Request Successful"
    //     });
    // }

    /// <summary>
    /// [Admin] Endpoint for Admin edit wallet.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of a wallet.</param>
    /// <returns>A wallet within status 200 or error status.</returns>
    /// <response code="200">Returns wallet after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetWalletDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateWalletAsync(Guid id, [FromBody] UpdateWalletModel requestBody)
    {
        // try
        // {
            GetWalletDetail updateWallet = await _walletService.UpdateWalletAsync(id, requestBody);

            return Ok(new BaseResponse<GetWalletDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateWallet,
                Msg = "Update Successful"
            });
        // }
        // catch (Exception e)
        // {
        //     return BadRequest(e);
        // }
            
    }
    
    /// <summary>
    /// [Admin] Endpoint for Admin Delete a wallet.
    /// </summary>
    /// <param name="id">ID of wallet</param>
    /// <returns>A wallet within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _walletService.DeleteWalletAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}