namespace ITJob.Services.Utility.ErrorHandling.Object;

public class CException : Exception
{
    /// <summary>
    /// Gets exception Message.
    /// </summary>
    public ErrorMessage? ErrorMessage { get; }

    /// <summary>
    /// Gets http Status Code.
    /// </summary>
    public int StatusCode { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CException"/> class.
    /// </summary>
    /// <param name="statusCode">http status code.</param>
    /// <param name="errorMessage">error custom Msg.</param>
    /// <param name="target">target error.</param>
    public CException(int statusCode, string errorMessage, string target = "")
    {
        this.ErrorMessage = new ErrorMessage
        {
            Message = errorMessage,
            Target = System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(target),
        };

        this.StatusCode = statusCode;
    }

    /// <inheritdoc/>
    public override string Message => string.IsNullOrEmpty(this.ErrorMessage.Message)
        ? "Undefined Error!"
        : this.ErrorMessage.Message;
}