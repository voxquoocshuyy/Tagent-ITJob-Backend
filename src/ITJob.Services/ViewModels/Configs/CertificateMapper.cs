using AutoMapper;
using ITJob.Services.ViewModels.Certificate;

namespace ITJob.Services.ViewModels.Configs;

public static class CertificateMapper
{
    public static void ConfigCertificate(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.Certificate, GetCertificateDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Certificate, CreateCertificateModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Certificate, UpdateCertificateModel>().ReverseMap();
    }
}