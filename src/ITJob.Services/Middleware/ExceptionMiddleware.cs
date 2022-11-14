using ITJob.Services.Utility.ErrorHandling;
using ITJob.Services.Utility.ErrorHandling.Object;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ITJob.Services.Middleware;

public class ExceptionMiddleware
{
    private readonly IConfiguration _configuration;
    private readonly RequestDelegate _requestDelegate;
    private readonly IErrorHandler _errorHandler;

    public ExceptionMiddleware(IConfiguration configuration, RequestDelegate requestDelegate,
        IErrorHandler errorHandler)
    {
        _configuration = configuration;
        _requestDelegate = requestDelegate;
        _errorHandler = errorHandler;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext == null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        await this._requestDelegate(httpContext);
        await this.HandleExceptionAsync(httpContext);
    }

    private async Task HandleExceptionAsync(HttpContext context)
    {
        IExceptionHandlerFeature? exceptionDetail = context.Features.Get<IExceptionHandlerFeature>();
        Exception? exception = exceptionDetail?.Error;

        if (exception == null)
        {
            return;
        }

        ValidationProblemDetails? exceptionMessage;
        exceptionMessage = exception is CException
            ? this._errorHandler.HandlerError(exception)
            : this._errorHandler.UnhandledError(exception);

        ErrorLocation errorLocation = _errorHandler.GetLocation(exception);
        // exceptionMessage.Instance = JsonConvert.SerializeObject(errorLocation, new JsonSerializerSettings()
        // {
        //     NullValueHandling = NullValueHandling.Ignore,
        //     ContractResolver = new CamelCasePropertyNamesContractResolver(),
        // });
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)exceptionMessage!.Status!;
        await context.Response.WriteAsync(JsonConvert.SerializeObject(exceptionMessage, new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        }));
    }
}