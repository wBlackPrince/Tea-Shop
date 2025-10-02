using Orders.Contracts;
using Orders.Contracts.Dtos;
using Shared.Abstractions;

namespace Orders.Application.Queries.GetOrderByIdQuery;

public record GetOrderByIdQuery(GetOrderRequestDto Request): IQuery;