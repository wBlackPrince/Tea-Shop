using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Domain.Subscriptions;

/// <summary>
/// Domain-модель подписки
/// </summary>
public class Subscription: Entity
{
    public Subscription(
        SubscriptionId id,
        UserId userId,
        User user,
        int durationInMonths,
        DateTime createdAt,
        DateTime updatedAt,
        Kit kit)
    {
        Id = id;
        UserId = userId;
        User = user;
        KitId = kit.Id;
        State = new ActiveState(SubscriptionStatus.MONTHLY, durationInMonths);
        Kit = kit;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
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
    /// Gets or sets навигационное свойство пользователя.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Gets or sets идентификатор набора.
    /// </summary>
    public KitId KitId { get; set; }

    /// <summary>
    /// Gets or sets состояние подписки.
    /// </summary>
    public SubscriptionState State { get; set; }

    /// <summary>
    /// Gets or sets чайный набор.
    /// </summary>
    public Kit Kit { get; set; }

    /// <summary>
    /// Gets or sets время создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets время обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets время последнего заказа
    /// </summary>
    public DateTime LastOrder { get; set; }
}