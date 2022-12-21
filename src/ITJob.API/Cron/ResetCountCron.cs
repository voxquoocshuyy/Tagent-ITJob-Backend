using ITJob.Services.Services.ProfileApplicantServices;
using Quartz;

namespace ITJob.API.Cron;

/// <summary>
/// 
/// </summary>
public class ResetCountCron : IJob
{
    private readonly IProfileApplicantService _profileApplicantService;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="profileApplicantService"></param>
    public ResetCountCron(IProfileApplicantService profileApplicantService)
    {
        _profileApplicantService = profileApplicantService;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("--Reset count like share");
        await _profileApplicantService.ResetCount();
    }
}