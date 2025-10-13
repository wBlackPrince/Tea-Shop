using Tea_Shop.Domain.Social;

namespace Tea_Shop.Application.Social;

public interface ISocialRepository
{
    Task<Comment?> GetCommentById(Guid commentId, CancellationToken cancellationToken);

    Task<Guid?> CreateComment(Comment comment, CancellationToken cancellationToken);

    Task<Guid?> DeleteComment(CommentId commentId, CancellationToken cancellationToken);

    Task<Review?> GetReviewById(
        ReviewId orderId,
        CancellationToken cancellationToken);

    Task<Guid> CreateReview(Review review, CancellationToken cancellationToken);

    Task<Guid> DeleteReview(
        ReviewId reviewId,
        CancellationToken cancellationToken);
}