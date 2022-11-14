using ITJob.Services.Enum;
using ITJob.Services.Services.ProductServices;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="productService"></param>
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    /// <summary>
    /// [Guest] Endpoint for get all product with condition
    /// </summary>
    /// <param name="searchProductModel"></param>
    /// <param name="paginationModel">An object contains paging criteria</param>
    /// <returns>List of product</returns>
    /// <response code="200">Returns the list of product</response>
    /// <response code="204">Returns if list of product is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ModelsResponse<GetProductDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProduct(
        [FromQuery]PagingParam<ProductEnum.ProductSort> paginationModel, 
        [FromQuery]SearchProductModel searchProductModel)
    {
        IList<GetProductDetail> result = _productService.GetProductPage(paginationModel, searchProductModel);
        int total = await _productService.GetTotal();
        if (!result.Any())
        {
            return NoContent();
        }

        return Ok(new ModelsResponse<GetProductDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get jobPostSkill page success!",
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
    /// [Guest] Endpoint for get product by ID
    /// </summary>
    /// <param name="id">An id of product</param>
    /// <returns>List of product</returns>
    /// <response code="200">Returns the product</response>
    /// <response code="204">Returns if the product is not exist</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(BaseResponse<GetProductDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        GetProductDetail result = await _productService.GetProductById(id);
        
        return Ok(new BaseResponse<GetProductDetail>()
        {
            Code = StatusCodes.Status200OK,
            Msg = "Use API get jobPostSkill by id success!",
            Data = result
        });
    }
    
    /// <summary>
    /// [Admin] Endpoint for create product
    /// </summary>
    /// <param name="requestBody">An obj contains input info of an product.</param>
    /// <returns>A product within status 201 or error status.</returns>
    /// <response code="201">Returns the product</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetProductDetail>), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateProduct([FromForm] CreateProductModel requestBody)
    {
        var result = await _productService.CreateProductAsync(requestBody);

        return Created(string.Empty, new BaseResponse<GetProductDetail>()
        {
            Code = StatusCodes.Status201Created,
            Data = result,
            Msg = "Send Request Successful"
        });
    }

    /// <summary>
    /// [Admin] Endpoint for Admin edit product.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="requestBody">An obj contains update info of an product.</param>
    /// <returns>A product within status 200 or error status.</returns>
    /// <response code="200">Returns product after update</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPut("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    [ProducesResponseType(typeof(BaseResponse<GetProductDetail>), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProductAsync(Guid id, [FromForm] UpdateProductModel requestBody)
    {
        try
        {
            GetProductDetail updateProduct = await _productService.UpdateProductAsync(id, requestBody);

            return Ok(new BaseResponse<GetProductDetail>()
            {
                Code = StatusCodes.Status200OK,
                Data = updateProduct,
                Msg = "Update Successful"
            });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
            
    }
    
    /// <summary>
    /// [Admin] Endpoint for Admin Delete a product.
    /// </summary>
    /// <param name="id">ID of product</param>
    /// <returns>A product within status 200 or 204 status.</returns>
    /// <response code="200">Returns 200 status</response>
    /// <response code="204">Returns NoContent status</response>
    [HttpDelete("{id}")]
    // [Authorize(Roles = RolesConstants.ADMIN)]
    public async Task<IActionResult> DeleteClassAsync(Guid id)
    {
        try
        {
            await _productService.DeleteProductAsync(id);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
        return NoContent();
    }
}