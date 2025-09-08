using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Users;

namespace Tea_Shop.Application.Users.Queries.GetUsersQuery;

public record GetUsersQuery(GetUsersRequestDto Request): IQuery;