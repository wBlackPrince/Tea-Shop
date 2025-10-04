namespace Orders.Contracts.Dtos;

public record OrderItemResponseDto(
    Guid ProductId,
    int Quantity);