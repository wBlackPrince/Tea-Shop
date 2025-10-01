namespace Baskets.Contracts;

public record AddBasketItemDto(
    Guid BusketId,
    Guid ProductId,
    int Quantity);