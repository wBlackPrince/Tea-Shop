namespace Tea_Shop.Contract.Orders;

public record OrderItemDto(
    Guid ProductId,
    int Quantity);