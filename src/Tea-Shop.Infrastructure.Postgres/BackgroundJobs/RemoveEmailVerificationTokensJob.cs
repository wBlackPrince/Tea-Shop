using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Tea_Shop.Domain.Subscriptions;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.BackgroundJobs;

public class RemoveEmailVerificationTokensJob(ServiceProvider serviceProvider): IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();

            await dbContext.EmailVerificationTokens.ExecuteDeleteAsync();

            await dbContext.SaveChangesAsync();
        }
    }
}