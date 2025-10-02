using Comments.Application;
using Comments.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.ValueObjects;

namespace Commnets.Infrastructure.Postgres;

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
}