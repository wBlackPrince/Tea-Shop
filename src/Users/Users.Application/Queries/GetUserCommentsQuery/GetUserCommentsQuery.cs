using Shared.Abstractions;
using Users.Contracts;
using Users.Contracts.Dtos;

namespace Users.Application.Queries.GetUserCommentsQuery;

public record GetUserCommentsQuery(GetUserWithPaginationRequestDto Request): IQuery;