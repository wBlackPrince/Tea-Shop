namespace Tea_Shop.Contract.Orders;

public record CreateOrderResponseDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public string DeliveryAddress { get; init; }

    public string PaymentMethod { get; init; }

    public string Status { get; init; }

    public DateTime ExpectedTimeDelivery { get; init; }

    public OrderItemDto[] Items { get; init; }
}