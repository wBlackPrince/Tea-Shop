namespace Orders.Contracts.Dtos;

public record CreateOrderRequestDto(
    Guid UserId,
    string DeliveryAddress,
    string PaymentMethod,
    DateTime ExpectedTimeDelivery,
    int UsedBonuses,
    OrderItemRequestDto[] Items);