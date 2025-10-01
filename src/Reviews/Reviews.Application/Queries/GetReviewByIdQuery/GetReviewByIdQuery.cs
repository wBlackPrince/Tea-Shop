using Reviews.Contracts;
using Shared.Abstractions;

namespace Reviews.Application.Queries.GetReviewByIdQuery;

public record GetReviewByIdQuery(GetReviewRequestDto Request): IQuery;