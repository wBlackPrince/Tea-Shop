using Tea_Shop.Domain.Comments;

namespace Tea_Shop.Application.Comments;

public interface ICommentsRepository
{
    Task<Comment?> GetCommentById(Guid commentId, CancellationToken cancellationToken);

    Task<Guid?> CreateComment(Comment comment, CancellationToken cancellationToken);

    Task<Guid?> DeleteComment(CommentId commentId, CancellationToken cancellationToken);
}