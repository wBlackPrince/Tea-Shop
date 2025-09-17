using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Database;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Comments.Commands.UpdateCommentCommand;

public class UpdateCommentHandler
{
    private readonly ICommentsRepository _commentsRepository;
    private readonly ILogger<UpdateCommentHandler> _logger;

    public UpdateCommentHandler(
        ICommentsRepository commentsRepository,
        ILogger<UpdateCommentHandler> logger)
    {
        _commentsRepository = commentsRepository;
        _logger = logger;
    }

    public async Task<Result<Guid?, Error>> Handle(
        Guid commentId,
        JsonPatchDocument<Comment> commentUpdates,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handlerName}", nameof(UpdateCommentHandler));
        Comment? comment = await _commentsRepository.GetCommentById(commentId, cancellationToken);

        if (comment is null)
        {
            _logger.LogError("Comment with id {commentId} does not exist", commentId);
            return Error.NotFound("comment.update", "comment not found");
        }

        try
        {
            commentUpdates.ApplyTo(comment);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "validation error while updating comment");
            return Error.Validation("comment.update", e.Message);
        }

        comment.UpdatedAt = DateTime.UtcNow.ToUniversalTime();

        await _commentsRepository.SaveChangesAsync(cancellationToken);

        _logger.LogDebug("Updated comment with id {commentId}", commentId);

        return comment.Id.Value;
    }
}