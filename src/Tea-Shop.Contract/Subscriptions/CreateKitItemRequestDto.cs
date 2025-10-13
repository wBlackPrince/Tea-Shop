namespace Tea_Shop.Contract.Subscriptions;

public record CreateKitItemRequestDto(
    Guid ProductId,
    int Amount);