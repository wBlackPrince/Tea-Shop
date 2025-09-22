namespace Tea_Shop.Contract.Orders;

public record OrderItemDto(
    Guid BasketItemId,
    int Quantity);