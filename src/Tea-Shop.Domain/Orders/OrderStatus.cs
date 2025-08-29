namespace Tea_Shop.Domain.Orders;

/// <summary>
/// Статус заказа в минимальном рабочем процессе
/// </summary>
public enum OrderStatus
{
    /// <summary> Заказ создан, но не оплачен. </summary>
    Pending,

    /// <summary> Оплата подтверждена, заказ готовится. </summary>
    Processing,

    /// <summary> Передан в доставку/ожидает получения. </summary>
    Shipped,

    /// <summary> Заказ получен клиентом. </summary>
    Delivered,

    /// <summary> Заказ отменен. </summary>
    Canceled
}