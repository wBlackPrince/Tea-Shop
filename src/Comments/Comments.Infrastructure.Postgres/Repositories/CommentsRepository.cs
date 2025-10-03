using Comments.Application;
using Comments.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.ValueObjects;

namespace Commnets.Infrastructure.Postgres.Repositories;

public class CommentsRepository: ICommentsRepository
{
    private readonly CommentsDbContext _dbContext;

    public CommentsRepository(CommentsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Comment?> GetCommentById(Guid commentId, CancellationToken cancellationToken)
    {
        var comment = await _dbContext.Comments.FirstOrDefaultAsync(
            c => c.Id == new CommentId(commentId),
            cancellationToken);

        return comment;
    }

    public async Task<Guid?> CreateComment(Comment comment, CancellationToken cancellationToken)
    {
        await _dbContext.Comments.AddAsync(comment, cancellationToken);

        return comment.Id.Value;
    }

    public async Task<Guid?> DeleteComment(CommentId commentId, CancellationToken cancellationToken)
    {
        await _dbContext.Comments
            .Where(c => c.Id == commentId)
            .ExecuteDeleteAsync(cancellationToken);

        return commentId.Value;
    }
    
    public async Task<Review?> GetReviewById(
        ReviewId orderId,
        CancellationToken cancellationToken)
    {
        Review? review = await _dbContext.Reviews.FirstOrDefaultAsync(
            o => o.Id == orderId,
            cancellationToken);

        return review;
    }

    public async Task<Guid> CreateReview(Review review, CancellationToken cancellationToken)
    {
        await _dbContext.Reviews.AddAsync(review, cancellationToken);

        return review.Id.Value;
    }

    public async Task<Guid> DeleteReview(
        ReviewId reviewId,
        CancellationToken cancellationToken)
    {
        await _dbContext.Reviews
            .Where(r => r.Id == reviewId)
            .ExecuteDeleteAsync(cancellationToken);

        return reviewId.Value;
    }
}