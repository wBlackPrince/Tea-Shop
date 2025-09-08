using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Users;

namespace Tea_Shop.Application.Users.Queries.GetUserByIdQuery;

public record GetUserByIdQuery(UserWithOnlyIdDto Request): IQuery;