using Orders.Contracts;
using Shared.Abstractions;

namespace Orders.Application.Queries.GetOrderByIdQuery;

public record GetOrderByIdQuery(GetOrderRequestDto Request): IQuery;