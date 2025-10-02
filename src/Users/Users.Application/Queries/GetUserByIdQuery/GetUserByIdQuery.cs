using Shared.Abstractions;
using Users.Contracts;
using Users.Contracts.Dtos;

namespace Users.Application.Queries.GetUserByIdQuery;

public record GetUserByIdQuery(UserWithOnlyIdDto Request): IQuery;