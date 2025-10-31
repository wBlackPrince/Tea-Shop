using Microsoft.Extensions.Options;
using Quartz;

namespace Tea_Shop.Infrastructure.Postgres.BackgroundJobs;

public class RemoveEmailVerificationTokensJobSetup: IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(RemoveEmailVerificationTokensJob));

        options
            .AddJob<RemoveEmailVerificationTokensJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger => trigger
                .ForJob(jobKey)
                .WithSimpleSchedule(schedule => schedule.WithIntervalInHours(6).RepeatForever()));
    }
}