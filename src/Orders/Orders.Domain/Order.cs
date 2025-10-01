using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using Shared;
using Shared.ValueObjects;
using Entity = Shared.Entity;

namespace Orders.Domain;

/// <summary>
/// Domain-модель заказа
/// </summary>
public class Order: Entity
{
    private string _deliveryAddress;
    private readonly List<OrderItem> _orderItems;

    /// <summary>
    /// Initializes a new instance of the <see cref="Order"/> class.
    /// </summary>
    /// <param name="id">Идентификатор заказа.</param>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="deliveryAddress">Адрес доставки.</param>
    /// <param name="paymentWay">Способ оплаты.</param>
    /// <param name="expectedDeliveryTime">Ожидаемое время доставки.</param>
    /// <param name="orderItems">Список заказанных товаров.</param>
    /// <param name="createdAt">Дата создания.</param>
    /// <param name="updatedAt">Дата обновления.</param>
    public Order(
        OrderId id,
        UserId userId,
        string deliveryAddress,
        PaymentWay paymentWay,
        DateTime expectedDeliveryTime,
        IEnumerable<OrderItem> orderItems,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        UserId = userId;
        DeliveryAddress = deliveryAddress;
        PaymentWay = paymentWay;
        ExpectedDeliveryTime = expectedDeliveryTime;
        OrderStatus = OrderStatus.Pending;
        _orderItems = orderItems.ToList();
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    // Для Ef Core
    private Order()
    {
    }

    /// <summary>
    /// Gets идентификатор заказа
    /// </summary>
    public OrderId Id { get; init; }

    /// <summary>
    /// Gets идентификатор пользователя
    /// </summary>
    public UserId UserId { get; init; }

    /// <summary>
    /// Gets or sets адрес доставки
    /// </summary>
    public string DeliveryAddress
    {
        get => _deliveryAddress;
        set => UpdateDeliveryAddress(value);
    }

    /// <summary>
    /// Gets or sets способ оплаты
    /// </summary>
    public PaymentWay PaymentWay { get; set; }

    /// <summary>
    /// Gets or sets ожидаемая дата доставки
    /// </summary>
    public DateTime ExpectedDeliveryTime { get; set; }

    /// <summary>
    /// Gets статус заказа
    /// </summary>
    public OrderStatus OrderStatus { get; private set; }

    /// <summary>
    /// Gets список доставочных товаров
    /// </summary>
    public IReadOnlyList<OrderItem> OrderItems => _orderItems;

    public int OrderItemsCount => _orderItems.Count;

    /// <summary>
    /// Gets or sets время создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets время обновления
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


    public void UpdateDeliveryAddress(string deliveryAddress)
    {
        var validationResult = CheckAttributeIsNotEmpty(deliveryAddress);

        if (validationResult.IsFailure)
        {
            throw new ValidationException("Delivery address cannot be empty");
        }

        _deliveryAddress = deliveryAddress;
    }

    public void UpdateStatus(OrderStatus orderStatus)
    {
        if (OrderStatus == OrderStatus.Delivered)
        {
            throw new ValidationException("Order is already delivered");
        }

        OrderStatus = orderStatus;
    }
}