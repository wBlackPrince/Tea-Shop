using Reviews.Contracts;
using Shared.Abstractions;

namespace Reviews.Application.Queries.GetReviewCommentsQuery;

public record GetReviewCommentsQuery(GetReviewRequestDto Request): IQuery;