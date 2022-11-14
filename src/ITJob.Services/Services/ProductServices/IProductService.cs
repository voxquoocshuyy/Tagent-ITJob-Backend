using ITJob.Services.Enum;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Product;

namespace ITJob.Services.Services.ProductServices;

public interface IProductService
{
    IList<GetProductDetail> GetProductPage(PagingParam<ProductEnum.ProductSort> paginationModel, SearchProductModel searchProductModel);

    public Task<GetProductDetail> GetProductById(Guid id);

    public Task<GetProductDetail> CreateProductAsync(CreateProductModel requestBody);

    public Task<GetProductDetail> UpdateProductAsync(Guid id, UpdateProductModel requestBody);

    public Task DeleteProductAsync(Guid id);

    public Task<int> GetTotal();   
}