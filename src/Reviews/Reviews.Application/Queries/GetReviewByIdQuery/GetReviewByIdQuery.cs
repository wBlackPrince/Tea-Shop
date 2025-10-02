using Reviews.Contracts;
using Reviews.Contracts.Dtos;
using Shared.Abstractions;

namespace Reviews.Application.Queries.GetReviewByIdQuery;

public record GetReviewByIdQuery(GetReviewRequestDto Request): IQuery;