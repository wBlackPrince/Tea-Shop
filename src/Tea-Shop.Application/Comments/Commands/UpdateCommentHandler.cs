using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Database;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Comments.Commands;

public class UpdateCommentHandler
{
    private readonly IReadDbContext _readDbContext;
    private readonly ICommentsRepository _commentsRepository;

    public UpdateCommentHandler(
        IReadDbContext readDbContext,
        ICommentsRepository commentsRepository)
    {
        _readDbContext = readDbContext;
        _commentsRepository = commentsRepository;
    }

    public async Task<Result<Guid?, Error>> Handle(
        Guid commentId,
        JsonPatchDocument<Comment> commentUpdates,
        CancellationToken cancellationToken)
    {
        Comment? comment = await _commentsRepository.GetCommentById(commentId, cancellationToken);

        if (comment is null)
        {
            return Error.NotFound("comment.update", "comment not found");
        }

        commentUpdates.ApplyTo(comment);
        await _commentsRepository.SaveChangesAsync(cancellationToken);

        return comment.Id.Value;
    }
}