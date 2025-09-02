using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Database;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Comments.Commands.DeleteCommentCommand;

public class DeleteCommentHandler
{
    private readonly ICommentsRepository _commentsRepository;
    private readonly IReadDbContext _readDbContext;

    public DeleteCommentHandler(
        ICommentsRepository commentsRepository,
        IReadDbContext readDbContext)
    {
        _commentsRepository = commentsRepository;
        _readDbContext = readDbContext;
    }

    public async Task<Result<Guid, Error>> Handle(
        Guid commentId,
        CancellationToken cancellationToken)
    {
        var comment = await _readDbContext.CommentsRead.FirstOrDefaultAsync(
            c => c.Id == new CommentId(commentId),
            cancellationToken);

        if (comment is null)
        {
            return Error.Failure("delete.comment", "Comment not found");
        }

        await _commentsRepository.DeleteComment(new CommentId(commentId), cancellationToken);

        await _commentsRepository.SaveChangesAsync(cancellationToken);

        return commentId;
    }
}