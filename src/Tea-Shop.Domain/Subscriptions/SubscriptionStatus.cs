namespace Tea_Shop.Domain.Subscriptions;

public enum SubscriptionStatus
{
    /// <summary> Подписка действует ежемесячно. </summary>
    MONTHLY,

    /// <summary> Полписка пропускается на определенное время. </summary>
    SKIP,

    /// <summary> Подписка остановлена на неопределенное время. </summary>
    PAUSE,
}