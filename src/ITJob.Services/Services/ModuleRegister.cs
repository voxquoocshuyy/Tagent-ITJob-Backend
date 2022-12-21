using ITJob.Services.Services.AlbumImageServices;
using ITJob.Services.Services.ApplicantServices;
using ITJob.Services.Services.BlockServices;
using ITJob.Services.Services.CertificateServices;
using ITJob.Services.Services.CompanyServices;
using ITJob.Services.Services.ConfigurationServices;
using ITJob.Services.Services.ConfirmMailServices;
using ITJob.Services.Services.EmployeeServices;
using ITJob.Services.Services.FileServices;
using ITJob.Services.Services.JobPositionServices;
using ITJob.Services.Services.JobPostServices;
using ITJob.Services.Services.JobPostSkillServices;
using ITJob.Services.Services.LikeServices;
using ITJob.Services.Services.MailServices;
using ITJob.Services.Services.MatchServices;
using ITJob.Services.Services.ProductServices;
using ITJob.Services.Services.ProfileApplicantServices;
using ITJob.Services.Services.ProfileApplicantSkillServices;
using ITJob.Services.Services.ProjectServices;
using ITJob.Services.Services.RoleServices;
using ITJob.Services.Services.SendSMSServices;
using ITJob.Services.Services.SkillGroupServices;
using ITJob.Services.Services.SkillLevelServices;
using ITJob.Services.Services.SkillServices;
using ITJob.Services.Services.TransactionJobPostServices;
using ITJob.Services.Services.TransactionServices;
using ITJob.Services.Services.UserServices;
using ITJob.Services.Services.WalletServices;
using ITJob.Services.Services.WorkingExperienceServices;
using ITJob.Services.Services.WorkingStyleServices;
using ITJob.Services.Utility;
using Microsoft.Extensions.DependencyInjection;

namespace ITJob.Services.Services;

public static class ModuleRegister
{
    public static void RegisterService(this IServiceCollection services)
    {
        services.AddScoped<ISkillGroupService, SkillGroupService>();
        services.AddScoped<IApplicantService, ApplicantService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<ISkillLevelService, SkillLevelService>();
        services.AddScoped<ICertificateService, CertificateService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtHelper, JwtHelper>();
        services.AddScoped<IJobPositionService, JobPositionService>();
        services.AddScoped<IWorkingStyleService, WorkingStyleService>();
        services.AddScoped<IProfileApplicantService, ProfileApplicantService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<IAlbumImageService, AlbumImageService>();
        services.AddScoped<IWorkingExperienceService, WorkingExperienceService>();
        services.AddScoped<IProfileApplicantSkillService, ProfileApplicantSkillService>();
        services.AddScoped<IJobPostService, JobPostService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<ILikeService, LikeService>();
        services.AddScoped<IBlockService, BlockService>();
        services.AddScoped<IJobPostSkillService, JobPostSkillService>();
        services.AddScoped<ISendSMSService, SendSMSService>();
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<IMatchService, MatchService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ITransactionJobPostService, TransactionJobPostService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IConfigurationService, ConfigurationService>();
    }
}