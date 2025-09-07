namespace Tea_Shop.Contract.Products;

public record GetProductsResponseDto(ProductDto[] Products, long TotalCount);