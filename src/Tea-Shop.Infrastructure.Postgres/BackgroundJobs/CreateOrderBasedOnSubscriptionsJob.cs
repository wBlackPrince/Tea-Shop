using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Quartz;

namespace Tea_Shop.Infrastructure.Postgres.BackgroundJobs;

public class CreateOrderBasedOnSubscriptionsJob(
    IHttpContextAccessor httpContextAccessor,
    LinkGenerator linkGenerator): IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        // получить юзеров у которых 

        return Task.CompletedTask;
    }
}