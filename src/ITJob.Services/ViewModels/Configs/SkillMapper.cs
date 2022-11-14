using AutoMapper;
using ITJob.Services.ViewModels.Skill;

namespace ITJob.Services.ViewModels.Configs;

public static class SkillMapper
{
    public static void ConfigSkill(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.Skill, GetSkillDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Skill, CreateSkillModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Skill, UpdateSkillModel>().ReverseMap();
    }
}