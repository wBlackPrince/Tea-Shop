namespace Baskets.Contracts.Dtos;

public record RemoveBasketItemDto(
    Guid BusketId,
    Guid BasketItemId);