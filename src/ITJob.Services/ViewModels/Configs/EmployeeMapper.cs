using AutoMapper;
using ITJob.Services.ViewModels.Employee;

namespace ITJob.Services.ViewModels.Configs;

public static class EmployeeMapper
{
    public static void ConfigEmployee(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.Employee, GetEmployeeDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Employee, CreateEmployeeModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Employee, UpdateEmployeeModel>().ReverseMap();
    }
}