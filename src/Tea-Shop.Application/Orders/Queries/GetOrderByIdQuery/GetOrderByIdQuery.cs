using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Orders;

namespace Tea_Shop.Application.Orders.Queries.GetOrderByIdQuery;

public record GetOrderByIdQuery(GetOrderRequestDto Request): IQuery;