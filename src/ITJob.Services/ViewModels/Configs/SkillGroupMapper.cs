using AutoMapper;
using ITJob.Services.ViewModels.SkillGroup;

namespace ITJob.Services.ViewModels.Configs;

public static class SkillGroupMapper
{
    public static void ConfigSkillGroup(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.SkillGroup, GetSkillGroupDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.SkillGroup, CreateSkillGroupModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.SkillGroup, UpdateSkillGroupModel>().ReverseMap();
    }
}