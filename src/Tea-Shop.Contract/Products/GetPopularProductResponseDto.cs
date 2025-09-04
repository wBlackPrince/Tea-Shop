namespace Tea_Shop.Contract.Products;

public record GetPopularProductResponseDto
{
    public Guid ProductId { get; init; }

    public string ProductName { get; init; }

    public int TotalOrderQuantity { get; init; }
}