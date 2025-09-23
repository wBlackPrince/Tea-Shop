using Tea_Shop.Contract.Products;

namespace Tea_Shop.Contract.Orders;

public record GetOrderResponseDto
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public string DeliveryAddress { get; init; }

    public string PaymentWay { get; init; }

    public DateTime ExpectedDeliveryTime { get; init; }

    public string OrderStatus { get; init; }

    public List<OrderItemResponseDto> OrderItems { get; set; } = new List<OrderItemResponseDto>();

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }
}