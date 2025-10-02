using Products.Contracts.Dtos;
using Shared.Abstractions;

namespace Products.Application.Queries.GetProductByIdQuery;

public record GetProductByIdQuery(ProductWithOnlyIdDto Request): IQuery;