using AutoMapper;
using ITJob.Services.ViewModels.Transaction;

namespace ITJob.Services.ViewModels.Configs;

public static class TransactionMapper
{
    public static void ConfigTransaction(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.Transaction, GetTransactionDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Transaction, CreateTransactionModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Transaction, UpdateTransactionModel>().ReverseMap();
    }
}