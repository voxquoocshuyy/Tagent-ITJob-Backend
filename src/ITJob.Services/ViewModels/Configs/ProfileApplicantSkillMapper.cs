using AutoMapper;
using ITJob.Services.ViewModels.ProfileApplicantSkill;

namespace ITJob.Services.ViewModels.Configs;

public static class ProfileApplicantSkillMapper
{
    public static void ConfigProfileApplicantSkill(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.ProfileApplicantSkill, GetProfileApplicantSkillDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.ProfileApplicantSkill, CreateProfileApplicantSkillModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.ProfileApplicantSkill, UpdateProfileApplicantSkillModel>().ReverseMap();
    }
}