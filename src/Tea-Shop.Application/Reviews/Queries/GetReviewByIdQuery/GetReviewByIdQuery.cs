using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Reviews;

namespace Tea_Shop.Application.Reviews.Queries.GetReviewByIdQuery;

public record GetReviewByIdQuery(GetReviewRequestDto Request): IQuery;