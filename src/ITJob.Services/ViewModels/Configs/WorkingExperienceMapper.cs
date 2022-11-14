using AutoMapper;
using ITJob.Services.ViewModels.WorkingExperience;

namespace ITJob.Services.ViewModels.Configs;

public static class WorkingExperienceMapper
{
    public static void ConfigWorkingExperience(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.WorkingExperience, GetWorkingExperienceDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.WorkingExperience, CreateWorkingExperienceModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.WorkingExperience, UpdateWorkingExperienceModel>().ReverseMap();
    }
}