using AutoMapper;
using ITJob.Services.ViewModels.SystemWallet;

namespace ITJob.Services.ViewModels.Configs;

public static class SystemWalletMapper
{
    public static void ConfigSystemWallet(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.SystemWallet, GetSystemWalletDetail>().ReverseMap();
    }
}