using Shared.ValueObjects;

namespace Subscriptions.Domain;

/// <summary>
/// Domain-модель подписки
/// </summary>
public class Subscription
{
    public Subscription(
        SubscriptionId id,
        UserId userId,
        int durationInMonths,
        Kit kit)
    {
        Id = id;
        UserId = userId;
        State = new ActiveState(SubscriptionStatus.MONTHLY, durationInMonths);
        Kit = kit;
    }

    // Для ef core
    private Subscription()
    {
    }

    /// <summary>
    /// Gets or sets идентификатор подписки.
    /// </summary>
    public SubscriptionId Id { get; set; }

    /// <summary>
    /// Gets or sets идентификатор пользователя.
    /// </summary>
    public UserId UserId { get; set; }

    /// <summary>
    /// Gets or sets состояние подписки.
    /// </summary>
    public SubscriptionState State { get; set; }

    /// <summary>
    /// Gets or sets чайный набор.
    /// </summary>
    public Kit Kit { get; set; }
}