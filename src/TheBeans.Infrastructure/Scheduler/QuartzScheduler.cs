using Quartz;
using Microsoft.Extensions.DependencyInjection;
using static Quartz.Logging.OperationName;
using TheBeans.Infrastructure.Services;


namespace TheBeans.Infrastructure.Scheduler
{
    public static class QuartzScheduler
    {
        public static IServiceCollection AddQuartzServices(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                var jobKey = new JobKey("BeanOfTheDayJob");
                q.AddJob<BeanOfTheDayJob>(opts => opts.WithIdentity(jobKey));
                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("BeanOfTheDayTrigger")
                    .WithCronSchedule("0 0 0 * * ?")); // Runs daily at midnight UTC
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            return services;
        }
    }
}