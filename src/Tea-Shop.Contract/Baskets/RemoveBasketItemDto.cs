namespace Tea_Shop.Contract.Baskets;

public record RemoveBasketItemDto(
    Guid BusketId,
    Guid BasketItemId);