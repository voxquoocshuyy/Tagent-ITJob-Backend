using System.Diagnostics;
using ITJob.Services.Utility.ErrorHandling.Object;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace ITJob.Services.Utility.ErrorHandling;

public class ErrorHandler : IErrorHandler
{
    public ValidationProblemDetails? HandlerError(Exception? exception)
    {
        if (exception == null || !(exception is CException cEx))
        {
            return null;
        }

        IDictionary<string, string[]> lisrError = new Dictionary<string, string[]>();

        if (cEx.ErrorMessage != null)
        {
            lisrError.Add(cEx.ErrorMessage.Target.ToString()!, new[] { cEx.ErrorMessage.Message });
        }

        var exceptionMessage = new ValidationProblemDetails(lisrError)
        {
            Status = cEx.StatusCode,
            Detail = cEx.Message,
            Title = "IT-Job - " + ReasonPhrases.GetReasonPhrase(cEx.StatusCode),
            Type = "https://tools.ietf.org/html/rfc7231",
        };

        return exceptionMessage;
    }

    /// <inheritdoc/>
    public ValidationProblemDetails? UnhandledError(Exception? exception)
    {
        if (exception == null)
        {
            return null;
        }

        var exceptionMessage = new ValidationProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231",
            Status = StatusCodes.Status500InternalServerError,
            Detail = "IT-Job - Contact Admin =))",
            Instance = exception.Message,
            Title = "IT-Job - " + ReasonPhrases.GetReasonPhrase(StatusCodes.Status500InternalServerError),
        };

        return exceptionMessage;
    }

    /// <inheritdoc/>
    public ErrorLocation? GetLocation(Exception? exception)
    {
        if (exception == null)
        {
            return null;
        }

        var trace = new StackTrace(exception, fNeedFileInfo: true);

        foreach (var mainFrame in trace.GetFrames())
        {
            if (!mainFrame.GetMethod()!.IsPublic || mainFrame.GetMethod()!.ContainsGenericParameters)
            {
                continue;
            }

            var el = new ErrorLocation
            {
                BugFile = mainFrame.GetFileName()?.Split("\\").LastOrDefault(),
                BugMethod = mainFrame.GetMethod()?.ToString(),
                BugLine = mainFrame.GetFileLineNumber(),
            };

            return el;
        }

        return null;
    }
}