using Shared.Abstractions;
using Users.Contracts;

namespace Users.Application.Queries.GetUserByIdQuery;

public record GetUserByIdQuery(UserWithOnlyIdDto Request): IQuery;