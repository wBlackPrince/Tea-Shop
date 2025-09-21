namespace Tea_Shop.Contract.Baskets;

public record AddBasketItemDto(
    Guid BusketId,
    Guid ProductId,
    int Quantity);