namespace Tea_Shop.Contract.Users;

public record RemoveBasketItemDto(
    Guid BusketId,
    Guid BasketItemId);