using Shared.Abstractions;
using Users.Contracts;

namespace Users.Application.Queries.GetUserReviewsQuery;

public record GetUserReviewsQuery(GetUserWithPaginationRequestDto Request): IQuery;