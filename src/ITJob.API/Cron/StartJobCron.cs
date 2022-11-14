using ITJob.Services.Services.JobPostServices;
using Quartz;

namespace ITJob.API.Cron;

/// <summary>
/// 
/// </summary>
public class StartJobCron : IJob
{
    private readonly IJobPostService _jobPostService;
    /// <summary>
    /// Initialized of instance <see cref="StartJobCron"/>
    /// </summary>
    /// <param name="jobPostService">Injection of <see cref="IJobPostService"/></param>
    public StartJobCron(IJobPostService jobPostService)
    {
        _jobPostService = jobPostService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("--Starting Job Post");
        await _jobPostService.StartJob();
    }
}