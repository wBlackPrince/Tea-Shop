namespace Tea_Shop.Contract.Products;

public record GetSeasonalProductsRequestDto(string Season, int ProductsLimit);