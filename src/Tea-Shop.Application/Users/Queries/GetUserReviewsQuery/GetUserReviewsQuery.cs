using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Users;

namespace Tea_Shop.Application.Users.Queries.GetUserReviewsQuery;

public record GetUserReviewsQuery(GetUserWithPaginationRequestDto Request): IQuery;