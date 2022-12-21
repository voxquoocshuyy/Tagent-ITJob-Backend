using ITJob.Services.Services.ConfigurationServices;
using ITJob.Services.ViewModels;
using ITJob.Services.ViewModels.Configuration;
using ITJob.Services.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITJob.API.Controllers;
/// <summary>
/// 
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/configurations")]
public class ConfigurationController : ControllerBase
{
    private readonly IConfigurationService _configurationService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configurationService"></param>
    public ConfigurationController(IConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }
    /// <summary>
    /// [Admin] Endpoint for admin get configuration 
    /// </summary>
    /// <returns>A configuration within status 201 or error status.</returns>
    /// <response code="201">Returns the configuration</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpGet]
    // [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(typeof(BaseResponse<CongfigurationModel>), StatusCodes.Status200OK)]
    public async Task<CongfigurationModel> GetConfiguration()
    {
        CongfigurationModel result = await _configurationService.GetConfiguration();
        return result;
    }
    
    /// <summary>
    /// [Admin] Endpoint for admin configuration 
    /// </summary>
    /// <param name="requestBody">An obj contains input info of a configuration.</param>
    /// <returns>A configuration within status 201 or error status.</returns>
    /// <response code="201">Returns the configuration</response>
    /// <response code="403">Return if token is access denied</response>
    [HttpPost]
    // [Authorize(Roles = "ADMIN")]
    public string Configuration([FromBody] CongfigurationModel requestBody)
    {
        _configurationService.Configuration(requestBody);
        return "Send Request Successful";
    }
}