using ITJob.Services.Services.JobPostServices;
using Quartz;

namespace ITJob.API.Cron;

/// <summary>
/// 
/// </summary>
public class OutOfMoneyCron : IJob
{
    private readonly IJobPostService _jobPostService;

    /// <summary>
    /// Initialized of instance <see cref="OutOfMoneyCron"/>
    /// </summary>
    /// <param name="jobPostService">Injection of <see cref="IJobPostService"/></param>
    public OutOfMoneyCron(IJobPostService jobPostService)
    {
        _jobPostService = jobPostService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("--Out Of Money Job Post");
        await _jobPostService.OutOfMoney();
    }
}