using AutoMapper;
using ITJob.Services.ViewModels.User;

namespace ITJob.Services.ViewModels.Configs;

public static class UserMapper
{
    public static void ConfigUser(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.User, GetUserDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.User, CreateUserModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.User, UpdateUserModel>().ReverseMap();
    }
}