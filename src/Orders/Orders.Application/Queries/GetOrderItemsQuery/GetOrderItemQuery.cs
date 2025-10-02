using Orders.Contracts;
using Orders.Contracts.Dtos;
using Shared.Abstractions;

namespace Orders.Application.Queries.GetOrderItemsQuery;

public record GetOrderItemQuery(GetOrderItemsRequestDto Request): IQuery;