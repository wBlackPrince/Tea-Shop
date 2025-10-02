namespace Products.Contracts.Dtos;

public record GetPopularProductsResponseDto
{
    public Guid ProductId { get; init; }

    public string ProductName { get; init; }

    public int TotalOrderQuantity { get; init; }
}