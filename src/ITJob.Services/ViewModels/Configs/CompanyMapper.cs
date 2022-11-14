using AutoMapper;
using ITJob.Services.ViewModels.Company;

namespace ITJob.Services.ViewModels.Configs;

public static class CompanyMapper
{
    public static void ConfigCompany(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.Company, GetCompanyDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Company, CreateCompanyModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Company, UpdateCompanyModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Company, UpgradeCompanyModel>().ReverseMap();
    }
}