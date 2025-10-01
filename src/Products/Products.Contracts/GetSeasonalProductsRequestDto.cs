namespace Products.Contracts;

public record GetSeasonalProductsRequestDto(string Season, int ProductsLimit);