namespace Tea_Shop.Contract.Products;

public record GetPopularProductRequestDto
{
    public int PopularProductsCount { get; init; }

    public DateTime StartSeasonDate { get; init; }

    public DateTime EndSeasonDate { get; init; }
}