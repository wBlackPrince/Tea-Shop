using Microsoft.Extensions.Logging;
using Quartz;

namespace Tea_Shop.Infrastructure.Postgres.BackgroundJobs;

[DisallowConcurrentExecution]
public class LoggingBackgroundJob(
    ILogger<LoggingBackgroundJob> logger): IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("{utcNow}", DateTime.UtcNow);

        return Task.CompletedTask;
    }
}