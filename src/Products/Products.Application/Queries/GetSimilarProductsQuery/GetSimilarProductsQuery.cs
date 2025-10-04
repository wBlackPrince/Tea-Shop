using Products.Contracts.Dtos;
using Shared.Abstractions;

namespace Products.Application.Queries.GetSimilarProductsQuery;

public record GetSimilarProductsQuery(ProductWithOnlyIdDto Request): IQuery;