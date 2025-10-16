namespace Tea_Shop.Contract.Orders;

public record CreateOrderBasedOnSubscriptionRequestDto(
    Guid UserId,
    Guid SubscriptionId,
    string DeliveryWay,
    string? DeliveryAddress = null);