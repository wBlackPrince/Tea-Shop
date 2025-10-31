using Microsoft.Extensions.Options;
using Quartz;

namespace Tea_Shop.Infrastructure.Postgres.BackgroundJobs;

public class CancelSubscriptionsJobSetup: IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(CancelSubscriptionsJob));

        options
            .AddJob<CancelSubscriptionsJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger => trigger
                .ForJob(jobKey)
                .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(10).RepeatForever()));
    }
}