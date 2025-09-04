using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products.Queries.GetSeasonalProductsQuery;

public record GetSeasonalProductsQuery(GetSeasonalProductsRequestDto Request): IQuery;