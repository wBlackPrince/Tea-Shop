using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Orders;

namespace Tea_Shop.Application.Orders.Queries.GetOrderItemsQuery;

public record GetOrderItemQuery(GetOrderItemsRequestDto Request): IQuery;