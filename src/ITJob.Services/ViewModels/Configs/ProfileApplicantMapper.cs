using AutoMapper;
using ITJob.Services.ViewModels.ProfileApplicant;

namespace ITJob.Services.ViewModels.Configs;

public static class ProfileApplicantMapper
{
    public static void ConfigProfileApplicant(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.ProfileApplicant, GetProfileApplicantDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.ProfileApplicant, CreateProfileApplicantModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.ProfileApplicant, UpdateProfileApplicantModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.ProfileApplicant, ProfileApplicantScore>().ReverseMap();
        configuration.CreateMap<GetProfileApplicantDetail, ProfileApplicantScore>().ReverseMap();
    }
}