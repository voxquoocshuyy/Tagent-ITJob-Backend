using ITJob.Entity.Repositories.AlbumImageRepositories;
using ITJob.Entity.Repositories.ApplicantRepositories;
using ITJob.Entity.Repositories.BlockRepositories;
using ITJob.Entity.Repositories.CertificateRepositories;
using ITJob.Entity.Repositories.CompanyRepositories;
using ITJob.Entity.Repositories.EmployeeRepositories;
using ITJob.Entity.Repositories.JobPositionRepositories;
using ITJob.Entity.Repositories.JobPostRepositories;
using ITJob.Entity.Repositories.JobPostSkillRepositories;
using ITJob.Entity.Repositories.LikeRepositories;
using ITJob.Entity.Repositories.ProductRepositories;
using ITJob.Entity.Repositories.ProfileApplicantRepositories;
using ITJob.Entity.Repositories.ProfileApplicantSkillRepositories;
using ITJob.Entity.Repositories.ProjectRepositories;
using ITJob.Entity.Repositories.RoleRepositories;
using ITJob.Entity.Repositories.SkillGroupRepositories;
using ITJob.Entity.Repositories.SkillLevelRepositories;
using ITJob.Entity.Repositories.SkillRepositories;
using ITJob.Entity.Repositories.SystemWalletRepositories;
using ITJob.Entity.Repositories.TransactionJobPostRepositories;
using ITJob.Entity.Repositories.TransactionRepositories;
using ITJob.Entity.Repositories.UserRepositories;
using ITJob.Entity.Repositories.WalletRepositories;
using ITJob.Entity.Repositories.WorkingExperienceRepositories;
using ITJob.Entity.Repositories.WorkingStyleRepositories;
using Microsoft.Extensions.DependencyInjection;

namespace ITJob.Entity.Repositories;

public static class ModuleRegister
{
    public static void RegisterRepository(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<ISkillGroupRepository, SkillGroupRepository>();
        services.AddScoped<IApplicantRepository, ApplicantRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ISkillLevelRepository, SkillLevelRepository>();
        services.AddScoped<ICertificateRepository, CertificateRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJobPositionRepository, JobPositionRepository>();
        services.AddScoped<IWorkingStyleRepository, WorkingStyleRepository>();
        services.AddScoped<IProfileApplicantRepository, ProfileApplicantRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ISkillRepository, SkillRepository>();
        services.AddScoped<IAlbumImageRepository, AlbumImageRepository>();
        services.AddScoped<IWorkingExperienceRepository, WorkingExperienceRepository>();
        services.AddScoped<IProfileApplicantSkillRepository, ProfileApplicantSkillRepository>();
        services.AddScoped<IJobPostRepository, JobPostRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<IBlockRepository, BlockRepository>();
        services.AddScoped<IJobPostSkillRepository, JobPostSkillRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ITransactionJobPostRepository, TransactionJobPostRepository>();
        services.AddScoped<ISystemWalletRepository, SystemWalletRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    }
}