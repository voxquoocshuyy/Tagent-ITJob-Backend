using AutoMapper;
using ITJob.Entity.Entities;
using ITJob.Entity.Repositories.ProductRepositories;
using ITJob.Services.Enum;
using ITJob.Services.Services.FileServices;
using ITJob.Services.Utility;
using ITJob.Services.Utility.ErrorHandling.Object;
using ITJob.Services.Utility.Paging;
using ITJob.Services.ViewModels.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITJob.Services.Services.ProductServices;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public ProductService(IProductRepository productRepository, IMapper mapper, IFileService fileService)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _fileService = fileService;
    }
    public IList<GetProductDetail> GetProductPage(PagingParam<ProductEnum.ProductSort> paginationModel, SearchProductModel searchProductModel)
    {
        IQueryable<Product> queryProduct = _productRepository.Table;
        queryProduct = queryProduct.GetWithSearch(searchProductModel);
        // Apply sort
        queryProduct = queryProduct.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
        // Apply Paging
        queryProduct = queryProduct.GetWithPaging(paginationModel.Page, paginationModel.PageSize).AsQueryable();
        var result = _mapper.ProjectTo<GetProductDetail>(queryProduct);
        return result.ToList();
    }
    
    public async Task<GetProductDetail> GetProductById(Guid id)
    {
        Product product = await _productRepository.GetFirstOrDefaultAsync(e => e.Id == id);
        if (product == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        var result = _mapper.Map<GetProductDetail>(product);
        return result;
    }

    public async Task<GetProductDetail> CreateProductAsync(CreateProductModel requestBody)
    {
        Product product = _mapper.Map<Product>(requestBody);
        if (product == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        if (requestBody.UploadFile == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Image of product is not null ");
        }
        product.Image = await _fileService.UploadFile(requestBody.UploadFile);
        await _productRepository.InsertAsync(product);
        await _productRepository.SaveChangesAsync();
        GetProductDetail productDetail = _mapper.Map<GetProductDetail>(product);
        return productDetail;
    }
    public async Task<GetProductDetail> UpdateProductAsync(Guid id, UpdateProductModel requestBody)
    {
        if (id != requestBody.Id)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        Product product = await _productRepository.GetFirstOrDefaultAsync(alu => alu.Id == requestBody.Id);
        if (product == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        if (requestBody.UploadFile == null)
        {
            product.Image = product.Image;
        }
        else
        {
            product.Image = await _fileService.UploadFile(requestBody.UploadFile);
        }
        product = _mapper.Map(requestBody, product);
        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync();
        GetProductDetail productDetail = _mapper.Map<GetProductDetail>(product);
        return productDetail;
    }

    public async Task DeleteProductAsync(Guid id)
    {
        Product? product = await _productRepository.GetFirstOrDefaultAsync(alu => alu.Id == id);
        if (product == null)
        {
            throw new CException(StatusCodes.Status400BadRequest, "Please enter the correct information!!! ");
        }
        product.Status = (int?)ProductEnum.ProductStatus.Inactive;
        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync();
    }

    public async Task<int> GetTotal()
    {
        return await _productRepository.GetAll().CountAsync();
    }
}