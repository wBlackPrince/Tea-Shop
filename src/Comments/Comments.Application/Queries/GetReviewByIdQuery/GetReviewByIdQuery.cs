using Comments.Contracts.Dtos;
using Shared.Abstractions;

namespace Comments.Application.Queries.GetReviewByIdQuery;

public record GetReviewByIdQuery(GetReviewRequestDto Request): IQuery;