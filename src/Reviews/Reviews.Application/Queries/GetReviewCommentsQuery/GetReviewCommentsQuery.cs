using Reviews.Contracts;
using Reviews.Contracts.Dtos;
using Shared.Abstractions;

namespace Reviews.Application.Queries.GetReviewCommentsQuery;

public record GetReviewCommentsQuery(GetReviewRequestDto Request): IQuery;