using Products.Contracts.Dtos;
using Shared.Abstractions;

namespace Products.Application.Queries.GetProductsQuery;

public record GetProductsQuery(GetProductsRequestDto Request): IQuery;