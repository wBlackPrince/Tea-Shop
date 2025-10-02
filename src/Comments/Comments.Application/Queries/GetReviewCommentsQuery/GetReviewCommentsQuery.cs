using Comments.Contracts.Dtos;
using Shared.Abstractions;

namespace Comments.Application.Queries.GetReviewCommentsQuery;

public record GetReviewCommentsQuery(GetReviewRequestDto Request): IQuery;