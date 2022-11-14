using AutoMapper;
using ITJob.Services.ViewModels.JobPosition;

namespace ITJob.Services.ViewModels.Configs;

public static class JobPositionMapper
{
    public static void ConfigJobPosition(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.JobPosition, GetJobPositionDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.JobPosition, CreateJobPositionModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.JobPosition, UpdateJobPositionModel>().ReverseMap();
    }
}