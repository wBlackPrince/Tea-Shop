using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products.Queries.GetProductsQuery;

public record GetProductsQuery(GetProductsRequestDto Request): IQuery;