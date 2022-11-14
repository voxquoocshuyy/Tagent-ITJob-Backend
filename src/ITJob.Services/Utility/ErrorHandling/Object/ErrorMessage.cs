namespace ITJob.Services.Utility.ErrorHandling.Object;

public class ErrorMessage
{
    /// <summary>
    /// Gets custom error message.
    /// </summary>
    public string Message { get; init; } = null!;

    /// <summary>
    /// Gets things meet error.
    /// </summary>
    public object Target { get; init; } = null!;
}