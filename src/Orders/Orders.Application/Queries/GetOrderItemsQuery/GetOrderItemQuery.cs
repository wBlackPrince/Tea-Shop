using Orders.Contracts;
using Shared.Abstractions;

namespace Orders.Application.Queries.GetOrderItemsQuery;

public record GetOrderItemQuery(GetOrderItemsRequestDto Request): IQuery;