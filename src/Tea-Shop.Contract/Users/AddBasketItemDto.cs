namespace Tea_Shop.Contract.Users;

public record AddBasketItemDto(
    Guid BusketId,
    Guid ProductId,
    int Quantity);