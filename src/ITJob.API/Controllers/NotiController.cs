using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/notis")]
public class NotiController : ControllerBase
{
    /// <summary>
    /// [Guest] Endpoint for company subscribe topic with condition
    /// </summary>
    /// <returns>Msg</returns>
    /// <response code="200">Returns msg</response>
    /// <response code="204">Returns msg is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost("subscribe")]
    [AllowAnonymous]
    public async Task<IActionResult> SubscribeTopic(IReadOnlyList<string> registrationToken, string topic)

    {
        // These registration tokens come from the client FCM SDKs.
        // Subscribe the devices corresponding to the registration tokens to the
        // topic
        var response = await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(
            registrationToken, topic);
        // See the TopicManagementResponse reference documentation
        // for the contents of response.
        Console.WriteLine($"{response.SuccessCount} tokens were subscribed successfully");
        return Ok(response.SuccessCount);
    }
    
    /// <summary>
    /// [Guest] Endpoint for company unsubscribe topic with condition
    /// </summary>
    /// <returns>Msg</returns>
    /// <response code="200">Returns msg</response>
    /// <response code="204">Returns msg is empty</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost("unsubscribe")]
    [AllowAnonymous]
    public async Task<IActionResult> UnSubscribeTopic(IReadOnlyList<string> registrationToken, string topic)

    {
        // Unsubscribe the devices corresponding to the registration tokens from the
        // topic
        var response = await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(
            registrationToken, topic);
        // See the TopicManagementResponse reference documentation
        // for the contents of response.
        Console.WriteLine($"{response.SuccessCount} tokens were unsubscribed successfully");
        return Ok(response.SuccessCount);
    }
}