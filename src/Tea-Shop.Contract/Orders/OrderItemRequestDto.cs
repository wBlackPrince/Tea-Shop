namespace Tea_Shop.Contract.Orders;

public record OrderItemRequestDto(
    Guid BasketItemId,
    int Quantity);