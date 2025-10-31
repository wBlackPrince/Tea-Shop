namespace Tea_Shop.Domain.Subscriptions;

public enum IntervalType
{
    /// <summary>
    /// Интервал измеряется в днях
    /// </summary>
    DAILY,

    /// <summary>
    /// Интервал измеряется в неделях
    /// </summary>
    WEEKLY,

    /// <summary>
    /// Интервал измеряется в месяцах
    /// </summary>
    MONTHLY,
}