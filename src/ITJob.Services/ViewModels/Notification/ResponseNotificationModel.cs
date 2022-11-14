using Newtonsoft.Json;

namespace ITJob.Services.ViewModels.Notification;

public class ResponseNotificationModel
{
    [JsonProperty("isSuccess")]
    public bool IsSuccess { get; set; }
    [JsonProperty("message")]
    public string Message { get; set; }
}