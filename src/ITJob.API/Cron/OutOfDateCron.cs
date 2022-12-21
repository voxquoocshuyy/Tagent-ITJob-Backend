using ITJob.Services.Services.JobPostServices;
using Quartz;

namespace ITJob.API.Cron;

/// <summary>
/// 
/// </summary>
public class OutOfDateCron : IJob
{
    private readonly IJobPostService _jobPostService;

    /// <summary>
    /// Initialized of instance <see cref="OutOfDateCron"/>
    /// </summary>
    /// <param name="jobPostService">Injection of <see cref="IJobPostService"/></param>
    public OutOfDateCron(IJobPostService jobPostService)
    {
        _jobPostService = jobPostService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("--Out Of Date Job Post");
        await _jobPostService.OutOfDateJob();
    }
}