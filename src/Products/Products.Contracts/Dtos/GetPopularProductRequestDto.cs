namespace Products.Contracts.Dtos;

public record GetPopularProductRequestDto
{
    public int PopularProductsCount { get; init; }

    public DateTime StartSeasonDate { get; init; }

    public DateTime EndSeasonDate { get; init; }
}