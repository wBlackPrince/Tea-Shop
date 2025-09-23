namespace Tea_Shop.Contract.Orders;

public record OrderItemResponseDto(
    Guid ProductId,
    int Quantity);