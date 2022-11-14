using Microsoft.Extensions.DependencyInjection;

namespace ITJob.Services.Utility.ErrorHandling;

public static class ModuleRegister
{
    /// <summary>
    /// DI Service classes for error handling.
    /// </summary>
    /// <param name="services">Service container from Program.</param>
    public static void RegisterErrorHandling(this IServiceCollection services)
    {
        services.AddTransient<IErrorHandler, ErrorHandler>();
    }
}