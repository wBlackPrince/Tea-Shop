using Shared.Abstractions;
using Users.Contracts;
using Users.Contracts.Dtos;

namespace Users.Application.Queries.GetUserReviewsQuery;

public record GetUserReviewsQuery(GetUserWithPaginationRequestDto Request): IQuery;