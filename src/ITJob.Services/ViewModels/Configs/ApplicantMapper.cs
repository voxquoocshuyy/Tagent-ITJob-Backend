using AutoMapper;
using ITJob.Services.ViewModels.Applicant;

namespace ITJob.Services.ViewModels.Configs;

public static class ApplicantMapper
{
    public static void ConfigApplicant(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.Applicant, GetApplicantDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Applicant, CreateApplicantModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.Applicant, UpdateApplicantModel>().ReverseMap();
    }
}