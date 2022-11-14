using AutoMapper;
using ITJob.Services.ViewModels.JobPostSkill;

namespace ITJob.Services.ViewModels.Configs;

public static class JobPostSkillSkillMapper
{
    public static void ConfigJobPostSkill(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.JobPostSkill, GetJobPostSkillDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.JobPostSkill, CreateJobPostSkillModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.JobPostSkill, UpdateJobPostSkillModel>().ReverseMap();
    }
}