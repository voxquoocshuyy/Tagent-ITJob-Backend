using ITJob.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ITJob.Entity;

public static class ModuleRegister
{
    public static IServiceCollection RegisterData(this IServiceCollection services)
    {
        services.AddScoped<DbContext, ITJobContext>();
        
        return services;
    }
}