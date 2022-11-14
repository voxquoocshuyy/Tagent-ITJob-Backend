using AutoMapper;
using ITJob.Services.ViewModels.Like;

namespace ITJob.Services.ViewModels.Configs;

public static class LikeMapper
{
    public static void ConfigLike(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.Like, GetLikeDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Like, CreateLikeModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Like, UpdateLikeModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Like, UpdateMatchModel>().ReverseMap();
    }
}