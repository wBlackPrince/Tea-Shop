namespace Orders.Contracts;

public record OrderItemRequestDto(
    Guid BasketItemId,
    int Quantity);