namespace Products.Contracts.Dtos;

public record GetSeasonalProductsRequestDto(string Season, int ProductsLimit);