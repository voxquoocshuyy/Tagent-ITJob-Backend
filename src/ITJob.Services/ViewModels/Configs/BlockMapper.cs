using AutoMapper;
using ITJob.Services.ViewModels.Block;

namespace ITJob.Services.ViewModels.Configs;

public static class BlockMapper
{
    public static void ConfigBlock(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.Block, GetBlockDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Block, CreateBlockModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Block, UpdateBlockModel>().ReverseMap();
    }
}