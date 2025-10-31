namespace Tea_Shop.Domain.Subscriptions;

public class SubscriptionState
{
    public SubscriptionState(
        IntervalType intervalType,
        int interval,
        int intervalBetweenOrders,
        int numberOfOrders)
    {
        IntervalType = intervalType;
        Interval = interval;
        IntervalBetweenOrders = intervalBetweenOrders;
        NumberOfOrders = numberOfOrders;
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
    public int IntervalBetweenOrders { get; set; }

    /// <summary>
    /// Gets or sets количество заказов в рамках подписки
    /// </summary>
    public int NumberOfOrders { get; set; }

    /// <summary>
    /// Gets or sets активность подписки
    /// </summary>
    public bool IsActive { get; set; }
}