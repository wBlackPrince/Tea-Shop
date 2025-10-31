using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Orders;
using Tea_Shop.Application.Subscriptions;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Subscriptions;
using Tea_Shop.Domain.Users;


namespace Tea_Shop.Infrastructure.Postgres.BackgroundJobs;

public record SubInfoForOrderGuids
{
    public Guid SubscriptionId { get; init; }

    public Guid UserId { get; init; }
};


public record SubInfoForOrder
{
    public SubscriptionId SubscriptionId { get; init; }

    public UserId UserId { get; init; }
};

[DisallowConcurrentExecution]
public class CreateOrderBasedOnSubscriptionsJob(
    IServiceProvider serviceProvider): IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        // получаем подписки по которым нужно сформировать заказы
        var subscriptions = await GetSubscriptionsForOrders();


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
                await CreateOrderBasedOnSubscriptions(sub);
            }

            await dbContext.SaveChangesAsync();
        }
    }


    public async Task CreateOrderBasedOnSubscriptions(Subscription sub)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var dbConnectionFactory = scope.ServiceProvider.GetRequiredService<IDbConnectionFactory>();
            var dbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
            string? adress = sub.User.Address;

            if (adress is null)
            {
                using (var connect = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None))
                {

                    var lastOrderDeliveryPlace = await connect.QueryAsync<string>(
                        """
                        select o.delivery_address
                        from users as u join orders as o on u.id = o.user_id
                        where u.id = @userId
                        order by updated_at desc
                        limit 1
                        """,
                        param: new { userId = sub.User.Id });

                    adress = lastOrderDeliveryPlace.First();
                }
            }


            using var innerScope = serviceProvider.CreateScope();
            var transactionManager = scope.ServiceProvider.GetRequiredService<ITransactionManager>();
            var subscriptionsRepository = scope.ServiceProvider.GetRequiredService<ISubscriptionsRepository>();
            var ordersRepository = scope.ServiceProvider.GetRequiredService<IOrdersRepository>();

            var transactionScopeResult = await transactionManager.BeginTransactionAsync(
                IsolationLevel.RepeatableRead,
                CancellationToken.None);

            using var transactionScope = transactionScopeResult.Value;

            var userId = sub.UserId;
            var user = sub.User;

            var subscriptionId = sub.Id;
            var subscriptionWithKit = await subscriptionsRepository
                .GetSubscriptionWithKit(subscriptionId, CancellationToken.None);

            var orderItems = subscriptionWithKit.Kit.KitItems
                .Select(ki => OrderItem.Create(
                    new OrderItemId(Guid.NewGuid()),
                    ki.ProductId,
                    ki.Amount).Value).ToList();

            DateTime createdAt = DateTime.UtcNow;
            DateTime updatedAt = DateTime.UtcNow;
            DateTime expectedDate = CalculExpectedDeliveryTime.Calcul(createdAt, adress, DeliveryWay.Courier);
            DeliveryWay deliveryWay = DeliveryWay.Courier;

            var orderId = new OrderId(Guid.NewGuid());
            var order = new Order(
                orderId,
                userId,
                user.Address,
                PaymentWay.CardOnline,
                deliveryWay,
                expectedDate,
                orderItems,
                createdAt,
                updatedAt);

            await ordersRepository.CreateOrder(order, CancellationToken.None);


            var saveResult = await transactionManager.SaveChangesAsync(CancellationToken.None);

            if (saveResult.IsFailure)
            {
                transactionScope.Rollback();
            }


            var commitedResult = transactionScope.Commit();

            if (commitedResult.IsFailure)
            {
                transactionScope.Rollback();
            }


            float orderSum = 0;

            foreach (var orderItem in order.OrderItems)
            {
                var product = await dbContext.ProductsRead
                    .FirstOrDefaultAsync(p => p.Id == orderItem.ProductId);

                orderSum += product.Price * orderItem.Quantity;
            }
        }
    }


    public async Task<SubInfoForOrderGuids[]> GetSubscriptionsForOrders()
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
                                                                        where (s.last_order + (case s.interval_type
                                                                                                WHEN 'MONTHLY' THEN interval '1 month'
                                                                                                WHEN 'WEEKLY' THEN interval '1 week'
                                                                                                WHEN 'DAILY' THEN interval '1 day'
                                                                                               End) * s.status_duration)::date = current_date
                                                                        """)).ToArray();

            return subscriptions;
        }
    }
}