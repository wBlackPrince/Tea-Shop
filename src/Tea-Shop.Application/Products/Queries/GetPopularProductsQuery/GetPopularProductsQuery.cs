using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products.Queries.GetPopularProductsQuery;

public record GetPopularProductsQuery(GetPopularProductRequestDto Request): IQuery;