namespace Tea_Shop.Contract.Products;

public record GetSimpleProductResponseDto
{
    public Guid ProductId { get; init; }

    public string ProductName { get; init; }
}