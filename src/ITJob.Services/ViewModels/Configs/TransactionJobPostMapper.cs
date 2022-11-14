using AutoMapper;
using ITJob.Services.ViewModels.TransactionJobPost;

namespace ITJob.Services.ViewModels.Configs;

public static class TransactionJobPostMapper
{
    public static void ConfigTransactionJobPost(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.TransactionJobPost, GetTransactionJobPostDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.TransactionJobPost, CreateTransactionJobPostModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.TransactionJobPost, UpdateTransactionJobPostModel>().ReverseMap();
    }
}