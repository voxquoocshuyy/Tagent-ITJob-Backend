using AutoMapper;
using ITJob.Services.ViewModels.SkillLevel;

namespace ITJob.Services.ViewModels.Configs;

public static class SkillLevelMapper
{
    public static void ConfigSkillLevel(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.SkillLevel, GetSkillLevelDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.SkillLevel, CreateSkillLevelModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.SkillLevel, UpdateSkillLevelModel>().ReverseMap();
    }
}