namespace Tea_Shop.Contract.Subscriptions;

public record KitItemDto(
    Guid KitItemId,
    Guid KitId,
    Guid ProductId,
    int Amount);