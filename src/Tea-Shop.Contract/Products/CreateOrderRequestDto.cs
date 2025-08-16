namespace Tea_Shop.Contract.Products;

public record CreateOrderRequestDto(
    Guid UserId,
    string DeliveryAddress,
    string PaymentMethod,
    string Status,
    CreateOrderItemRequestDto[] Items);