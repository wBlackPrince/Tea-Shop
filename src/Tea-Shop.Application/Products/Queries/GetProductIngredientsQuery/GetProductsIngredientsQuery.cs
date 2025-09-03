using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products.Queries.GetProductIngredientsQuery;

public record GetProductsIngredientsQuery(GetProductIngridientsRequestDto Request): IQuery;