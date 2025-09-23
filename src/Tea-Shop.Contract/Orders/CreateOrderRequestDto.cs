namespace Tea_Shop.Contract.Orders;

public record CreateOrderRequestDto(
    Guid UserId,
    string DeliveryAddress,
    string PaymentMethod,
    DateTime ExpectedTimeDelivery,
    int UsedBonuses,
    OrderItemRequestDto[] Items);