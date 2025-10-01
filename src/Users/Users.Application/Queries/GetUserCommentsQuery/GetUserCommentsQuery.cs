using Shared.Abstractions;
using Users.Contracts;

namespace Users.Application.Queries.GetUserCommentsQuery;

public record GetUserCommentsQuery(GetUserWithPaginationRequestDto Request): IQuery;