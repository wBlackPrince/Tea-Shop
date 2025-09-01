using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Domain.Orders;

public record OrderId(Guid Value);

/// <summary>
/// Domain-модель заказа
/// </summary>
public class Order
{
    private readonly List<OrderItem> _orderItems;

    // Для Ef Core
    private Order() { }

    /// <summary>
    /// Initializes a new instance of the "Order" class.
    /// </summary>
    /// <param name="id">Идентификатор заказа.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="deliveryAddress">Адрес доставки.</param>
    /// <param name="paymentWay">Способ оплаты.</param>
    /// <param name="expectedDeliveryTime">Ожидаемое время доставки.</param>
    /// <param name="orderStatus">Статус заказа.</param>
    /// <param name="orderItems">Список заказанных товаров.</param>
    /// <param name="createdAt">Дата создания.</param>
    /// <param name="updatedAt">Дата обновления.</param>
    public Order(
        OrderId id,
        UserId userId,
        string deliveryAddress,
        PaymentWay paymentWay,
        DateTime expectedDeliveryTime,
        OrderStatus orderStatus,
        IEnumerable<OrderItem> orderItems,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        UserId = userId;
        DeliveryAddress = deliveryAddress;
        PaymentWay = paymentWay;
        ExpectedDeliveryTime = expectedDeliveryTime;
        OrderStatus = orderStatus;
        _orderItems = orderItems.ToList();
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Gets or sets идентификатор заказа
    /// </summary>
    public OrderId Id { get; set; }

    /// <summary>
    /// Gets or sets идентификатор пользователя
    /// </summary>
    public UserId UserId { get; set; }

    /// <summary>
    /// Gets or sets адрес доставки
    /// </summary>
    public string DeliveryAddress { get; set; }

    /// <summary>
    /// Gets or sets способ оплаты
    /// </summary>
    public PaymentWay PaymentWay { get; set; }

    /// <summary>
    /// Gets or sets ожидаемая дата доставки
    /// </summary>
    public DateTime ExpectedDeliveryTime { get; set; }

    /// <summary>
    /// Gets or sets статус заказа
    /// </summary>
    public OrderStatus OrderStatus { get; set; }

    /// <summary>
    /// Gets or sets список доставочных товаров
    /// </summary>
    public IReadOnlyList<OrderItem> OrderItems => _orderItems;

    public int OrderItemsCount => _orderItems.Count;

    /// <summary>
    /// Get or sets время создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Get or sets время обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }


    public UnitResult<Error> AddOrderItem(OrderItem orderItem)
    {
        if (OrderItemsCount > (int)OrdersConstants.ORDER_ITEMS_LIMIT)
        {
            return Error.Conflict("orders.orders_items.LIMIT", "Too many order items");
        }

        _orderItems.Add(orderItem);

        return UnitResult.Success<Error>();
    }
}