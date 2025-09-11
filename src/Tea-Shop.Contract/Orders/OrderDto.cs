namespace Tea_Shop.Contract.Orders;

public record OrderDto
{
    public Guid OrderId { get; init; }

    public string DeliveryAddress { get; init; } = string.Empty;

    public string PaymentWay { get; init; } = string.Empty;

    public DateTime ExpectedDeliveryTime { get; init; }

    public string OrderStatus { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}