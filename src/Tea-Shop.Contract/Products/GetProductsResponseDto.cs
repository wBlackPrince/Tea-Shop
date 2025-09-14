namespace Tea_Shop.Contract.Products;

public record GetProductsResponseDto
{
    public List<ProductDto> Products { get; init; } = [];

    public long TotalCount { get; set; }
}