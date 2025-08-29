using Tea_Shop.Contract.Products;

namespace Tea_Shop.Contract.Orders;

public record GetOrderResponseDto(
    Guid Id,
    Guid UserId,
    string DeliveryAddress,
    string PaymentWay,
    DateTime ExpectedDeliveryTime,
    string OrderStatus,
    OrderItemDto[] OrderItems,
    DateTime CreatedAt,
    DateTime UpdatedAt);