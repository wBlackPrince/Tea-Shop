namespace Orders.Contracts;

public record OrderItemResponseDto(
    Guid ProductId,
    int Quantity);