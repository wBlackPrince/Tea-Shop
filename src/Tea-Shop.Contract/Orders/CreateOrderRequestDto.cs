namespace Tea_Shop.Contract.Orders;

public record CreateOrderRequestDto(
    Guid UserId,
    string DeliveryAddress,
    string PaymentMethod,
    string Status,
    DateTime ExpectedTimeDelivery,
    OrderItemDto[] Items);