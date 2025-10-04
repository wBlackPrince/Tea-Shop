namespace Products.Contracts.Dtos;

public record GetProductsResponseDto
{
    public List<ProductDto> Products { get; init; } = [];

    public long TotalCount { get; set; }
}