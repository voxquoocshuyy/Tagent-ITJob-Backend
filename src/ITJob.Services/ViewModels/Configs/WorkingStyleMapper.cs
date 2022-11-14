using AutoMapper;
using ITJob.Services.ViewModels.WorkingStyle;

namespace ITJob.Services.ViewModels.Configs;

public static class WorkingStyleMapper
{
    public static void ConfigWorkingStyle(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.WorkingStyle, GetWorkingStyleDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.WorkingStyle, CreateWorkingStyleModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.WorkingStyle, UpdateWorkingStyleModel>().ReverseMap();
    }
}