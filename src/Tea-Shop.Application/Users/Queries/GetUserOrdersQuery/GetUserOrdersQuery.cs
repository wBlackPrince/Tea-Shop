using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Products;
using Tea_Shop.Contract.Users;

namespace Tea_Shop.Application.Users.Queries.GetUserOrdersQuery;

public record GetUserOrdersQuery(GetUserOrdersRequestDto Request): IQuery;