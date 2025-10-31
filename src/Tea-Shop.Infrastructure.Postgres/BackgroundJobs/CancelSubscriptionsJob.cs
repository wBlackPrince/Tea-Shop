using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Tea_Shop.Application.Database;
using Tea_Shop.Domain.Subscriptions;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.BackgroundJobs;

[DisallowConcurrentExecution]
public class CancelSubscriptionsJob(IServiceProvider serviceProvider): IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        // получаем подписки которые завершены
        var subscriptions = await GetFinishingSubscriptions();


        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();

            var userIds = subscriptions.Select(s => new UserId(s.UserId)).ToList();
            var subscriptionIds = subscriptions.Select(s => new SubscriptionId(s.SubscriptionId)).ToList();

            var entities = await dbContext.Subscriptions
                .Include(s => s.User)
                .Where(s => subscriptionIds.Contains(s.Id) && userIds.Contains(s.User.Id))
                .ToListAsync();

            // нельзя полагаться на httpContext
            foreach (var sub in entities)
            {
                sub.State.IsActive = false;
            }

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<SubInfoForOrderGuids[]> GetFinishingSubscriptions()
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
            var dbConnectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();

            var connection = await dbConnectionFactory.CreateConnectionAsync(default);

            var subscriptions = (connection.Query<SubInfoForOrderGuids>("""
                                                                        select
                                                                            u.id as user_id,
                                                                            s.id as subscription_id
                                                                        from users as u join subscriptions as s on u.id = s.user_id
                                                                        where (s.created_at + (case s.interval_type
                                                                                                WHEN 'MONTHLY' THEN interval '1 month'
                                                                                                WHEN 'WEEKLY' THEN interval '1 week'
                                                                                                WHEN 'DAILY' THEN interval '1 day'
                                                                                               End) * s.interval_between_orders)::date > current_date
                                                                        """)).ToArray();

            return subscriptions;
        }
    }
}