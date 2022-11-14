using Microsoft.AspNetCore.Builder;

namespace ITJob.Services.Middleware;

public static class ModuleRegister
{
    public static void UseExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        // app.UseHsts();
    }
}