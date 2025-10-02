using Products.Contracts.Dtos;
using Shared.Abstractions;

namespace Products.Application.Queries.GetProductIngredientsQuery;

public record GetProductsIngredientsQuery(GetProductIngridientsRequestDto Request): IQuery;