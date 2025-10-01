namespace Comments.Application;

public interface ICommentsRepository
{
    Task<Comment?> GetCommentById(Guid commentId, CancellationToken cancellationToken);

    Task<Guid?> CreateComment(Comment comment, CancellationToken cancellationToken);

    Task<Guid?> DeleteComment(CommentId commentId, CancellationToken cancellationToken);
}