namespace Tea_Shop.Contract.Products;

public record CreateOrderItemRequestDto(CreateProductRequestDto Product, int Quantity);