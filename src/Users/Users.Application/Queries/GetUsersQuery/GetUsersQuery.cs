using Shared.Abstractions;
using Users.Contracts;

namespace Users.Application.Queries.GetUsersQuery;

public record GetUsersQuery(GetUsersRequestDto Request): IQuery;