using AutoMapper;
using ITJob.Services.ViewModels.Wallet;

namespace ITJob.Services.ViewModels.Configs;

public static class WalletMapper
{
    public static void ConfigWallet(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.Wallet, GetWalletDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Wallet, CreateWalletModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Wallet, UpdateWalletModel>().ReverseMap();
    }
}