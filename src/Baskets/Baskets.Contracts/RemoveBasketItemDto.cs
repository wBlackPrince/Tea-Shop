namespace Baskets.Contracts;

public record RemoveBasketItemDto(
    Guid BusketId,
    Guid BasketItemId);