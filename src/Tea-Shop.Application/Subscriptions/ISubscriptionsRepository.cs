using Tea_Shop.Domain.Subscriptions;

namespace Tea_Shop.Application.Subscriptions;

public interface ISubscriptionsRepository
{
    Task<Guid> CreateKit(Kit kit, CancellationToken cancellationToken);

    Task<Subscription?> GetSubscriptionWithKit(
        SubscriptionId subscriptionId,
        CancellationToken cancellationToken);
}