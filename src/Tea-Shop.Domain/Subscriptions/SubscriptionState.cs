namespace Tea_Shop.Domain.Subscriptions;

public class SubscriptionState
{
    public SubscriptionState(
        IntervalType intervalType,
        int interval,
        int statusDuration)
    {
        IntervalType = intervalType;
        Interval = interval;
        StatusDuration = statusDuration;
        IsActive = true;
    }

    /// <summary>
    /// Gets or sets интервал подписки.
    /// </summary>
    public int Interval { get; set; }

    /// <summary>
    /// Gets or sets тип интервала подписки: дни, недели, месяцы.
    /// </summary>
    public IntervalType IntervalType { get; set; }

    /// <summary>
    /// Gets or sets длительность подписки.
    /// </summary>
    public int StatusDuration { get; set; }

    /// <summary>
    /// Gets or sets активность подписки
    /// </summary>
    public bool IsActive { get; set; }
}