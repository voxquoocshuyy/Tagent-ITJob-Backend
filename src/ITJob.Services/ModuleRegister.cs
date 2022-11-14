using ITJob.Entity.Repositories;
using ITJob.Services.Services;
using ITJob.Services.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ITJob.Services;

public static class ModuleRegister
{
    public static void RegisterBusiness(this IServiceCollection services)
    {
        services.RegisterRepository();
        services.RegisterService();
        services.ConfigureAutoMapper();
    }
}