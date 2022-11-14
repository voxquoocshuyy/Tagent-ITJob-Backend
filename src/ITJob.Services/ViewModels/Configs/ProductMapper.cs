using AutoMapper;
using ITJob.Services.ViewModels.Like;
using ITJob.Services.ViewModels.Product;

namespace ITJob.Services.ViewModels.Configs;

public static class ProductMapper
{
    public static void ConfigProduct(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.Product, GetProductDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Product, CreateProductModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Product, UpdateProductModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Product, UpdateMatchModel>().ReverseMap();
    }
}