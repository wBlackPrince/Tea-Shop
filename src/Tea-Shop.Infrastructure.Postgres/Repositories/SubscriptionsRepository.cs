using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Subscriptions;
using Tea_Shop.Domain.Subscriptions;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public class SubscriptionsRepository(ProductsDbContext dbContext): ISubscriptionsRepository
{
    public async Task<Subscription?> GetSubscriptionWithKit(
        SubscriptionId subscriptionId,
        CancellationToken cancellationToken)
    {
        var sub = await dbContext.Subscriptions
            .Include(s => s.Kit)
            .FirstOrDefaultAsync(s => s.Id == subscriptionId, cancellationToken);

        return sub;
    }

    public async Task<Guid> CreateKit(Kit kit, CancellationToken cancellationToken)
    {
        await dbContext.Kits.AddAsync(kit, cancellationToken);

        return kit.Id.Value;
    }
}