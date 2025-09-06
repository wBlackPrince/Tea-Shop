using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Reviews;

namespace Tea_Shop.Application.Reviews.Queries.GetReviewCommentsQuery;

public record GetReviewCommentsQuery(GetReviewRequestDto Request): IQuery;