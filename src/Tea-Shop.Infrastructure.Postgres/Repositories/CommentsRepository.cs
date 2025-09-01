using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Comments;
using Tea_Shop.Domain.Comments;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public class CommentsRepository: ICommentsRepository
{
    private readonly ProductsDbContext _dbContext;

    public CommentsRepository(ProductsDbContext dbContext)
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

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}