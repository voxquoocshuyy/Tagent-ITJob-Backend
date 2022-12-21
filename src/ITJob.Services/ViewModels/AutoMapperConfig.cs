using AutoMapper;
using ITJob.Services.ViewModels.Configs;
using Microsoft.Extensions.DependencyInjection;

namespace ITJob.Services.ViewModels;

public static class AutoMapperConfig
{
    public static void ConfigureAutoMapper(this IServiceCollection services)
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.ConfigApplicant();
            mc.ConfigRole();
            mc.ConfigSkillGroup();
            mc.ConfigUser();
            mc.ConfigCompany();
            mc.ConfigJobPosition();
            mc.ConfigSkillLevel();
            mc.ConfigWorkingStyle();
            mc.ConfigCertificate();
            mc.ConfigProfileApplicant();
            mc.ConfigProject();
            mc.ConfigSkill();
            mc.ConfigAlbumImage();
            mc.ConfigWorkingExperience();
            mc.ConfigProfileApplicantSkill();
            mc.ConfigJobPost();
            mc.ConfigLike();
            mc.ConfigBlock();
            mc.ConfigJobPostSkill();
            mc.ConfigProduct();
            mc.ConfigWallet();
            mc.ConfigTransaction();
            mc.ConfigTransactionJobPost();
            mc.ConfigSystemWallet();
            mc.ConfigEmployee();
        });

        IMapper mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);
    }
}