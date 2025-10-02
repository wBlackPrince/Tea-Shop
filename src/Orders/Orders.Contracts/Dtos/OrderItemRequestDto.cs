namespace Orders.Contracts.Dtos;

public record OrderItemRequestDto(
    Guid BasketItemId,
    int Quantity);