namespace Products.Contracts.Dtos;

public class GetSimilarProductResponseDto
{
    public Guid Id { get; init; }

    public string Title { get; init; }

    public float Price { get; init; }

    public float Amount { get; init; }

    public int StockQuantity { get; init; }

    public string Description { get; init; }

    public string Season { get; init; }
}