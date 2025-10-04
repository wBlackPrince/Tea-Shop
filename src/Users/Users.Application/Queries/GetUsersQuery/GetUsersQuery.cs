using Shared.Abstractions;
using Users.Contracts;
using Users.Contracts.Dtos;

namespace Users.Application.Queries.GetUsersQuery;

public record GetUsersQuery(GetUsersRequestDto Request): IQuery;