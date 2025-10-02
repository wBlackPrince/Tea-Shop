namespace Baskets.Contracts.Dtos;

public record AddBasketItemDto(
    Guid BusketId,
    Guid ProductId,
    int Quantity);