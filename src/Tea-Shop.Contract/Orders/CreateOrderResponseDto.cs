namespace Tea_Shop.Contract.Orders;

public record CreateOrderResponseDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public string DeliveryAddress { get; init; } = string.Empty;

    public string PaymentMethod { get; init; } = string.Empty;

    public string Status { get; init; } = string.Empty;

    public DateTime ExpectedTimeDelivery { get; init; }

    public float OrderSum { get; init; }

    public OrderItemResponseDto[] Items { get; init; } = [];
}