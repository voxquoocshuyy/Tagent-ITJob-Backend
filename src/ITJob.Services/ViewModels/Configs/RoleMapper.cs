using AutoMapper;
using ITJob.Services.ViewModels.Role;

namespace ITJob.Services.ViewModels.Configs;

public static class RoleMapper
{
    public static void ConfigRole(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.Role, GetRoleDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Role, CreateRoleModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Role, UpdateRoleModel>().ReverseMap();
    }
}