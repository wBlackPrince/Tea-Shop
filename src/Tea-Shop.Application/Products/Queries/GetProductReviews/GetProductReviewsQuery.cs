using Tea_Shop.Application.Abstractions;

namespace Tea_Shop.Application.Products.Queries.GetProductReviews;

public record GetProductReviewsQuery(Guid ProductId): IQuery;