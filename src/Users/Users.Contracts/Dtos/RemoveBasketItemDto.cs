namespace Users.Contracts.Dtos;

public record RemoveBasketItemDto(
    Guid BusketId,
    Guid BasketItemId);