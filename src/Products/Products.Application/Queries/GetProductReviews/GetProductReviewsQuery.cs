using Shared.Abstractions;

namespace Products.Application.Queries.GetProductReviews;

public record GetProductReviewsQuery(Guid ProductId): IQuery;