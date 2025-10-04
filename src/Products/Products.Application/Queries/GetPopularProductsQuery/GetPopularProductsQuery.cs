using Products.Contracts.Dtos;
using Shared.Abstractions;

namespace Products.Application.Queries.GetPopularProductsQuery;

public record GetPopularProductsQuery(GetPopularProductRequestDto Request): IQuery;