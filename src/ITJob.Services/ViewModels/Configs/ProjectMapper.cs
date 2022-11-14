using AutoMapper;
using ITJob.Services.ViewModels.Project;

namespace ITJob.Services.ViewModels.Configs;

public static class ProjectMapper
{
    public static void ConfigProject(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.Project, GetProjectDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Project, CreateProjectModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Project, UpdateProjectModel>().ReverseMap();
    }
}