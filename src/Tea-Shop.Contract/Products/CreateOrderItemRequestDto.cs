namespace Tea_Shop.Contract.Products;

public record CreateOrderItemRequestDto(Guid ProductId, int Quantity);