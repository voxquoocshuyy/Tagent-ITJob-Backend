using Quartz;

namespace ITJob.API.Cron;

/// <summary>
/// 
/// </summary>
public static class ModuleRegister
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    [Obsolete("Obsolete")]
    public static void RegisterQuartz(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory(opt =>
            {
                opt.AllowDefaultConstructor = true;
            });
            var startJobPost = new JobKey("StatJobPostCron", "JobPostGroup");
            var outOfDateJobPost = new JobKey("OutOfDateJobCron", "JobPostGroup");
            var resetCount = new JobKey("ResetCountCron", "JobPostGroup");
            // var outOfMoney = new JobKey("OutOfMoneyCron", "JobPostGroup");
            
            q.AddJob<StartJobCron>(o => o.WithIdentity(startJobPost));
            q.AddJob<OutOfDateCron>(o => o.WithIdentity(outOfDateJobPost));
            q.AddJob<ResetCountCron>(o=> o.WithIdentity(resetCount));
            // q.AddJob<OutOfMoneyCron>(o => o.WithIdentity(outOfMoney));
            
            q.AddTrigger(opts => opts.ForJob(startJobPost)
                .WithIdentity("StartJobPostTrigger")
                //0h0m0s every day
                .WithCronSchedule("0 0 0 ? * *"));
            
            q.AddTrigger(opts => opts.ForJob(outOfDateJobPost)
                .WithIdentity("OutOfDateJobPostTrigger")
                .WithCronSchedule("0 0 0 ? * *"));
            
            q.AddTrigger(opts => opts.ForJob(resetCount)
                .WithIdentity("ResetCountTrigger")
                .WithCronSchedule("0 0 0 ? * *"));
            
            // q.AddTrigger(opts => opts.ForJob(outOfMoney)
            //     .WithIdentity("OutOfMoneyTrigger")
            //     .WithCronSchedule("1 * * ? * *"));
            
            q.InterruptJobsOnShutdown = true;
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        services.AddTransient<IJob, StartJobCron>();
        services.AddTransient<IJob, OutOfDateCron>();
        services.AddTransient<IJob, ResetCountCron>();
        // services.AddTransient<IJob, OutOfMoneyCron>();
    }
}