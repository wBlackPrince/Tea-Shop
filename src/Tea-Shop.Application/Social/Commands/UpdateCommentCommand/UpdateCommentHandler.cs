using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Database;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Social.Commands.UpdateCommentCommand;

public class UpdateCommentHandler(
    ISocialRepository socialRepository,
    ILogger<UpdateCommentHandler> logger,
    ITransactionManager transactionManager)
{
    public async Task<Result<Guid?, Error>> Handle(
        Guid commentId,
        JsonPatchDocument<Comment> commentUpdates,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handlerName}", nameof(UpdateCommentHandler));

        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while creating product");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        Comment? comment = await socialRepository.GetCommentById(commentId, cancellationToken);

        if (comment is null)
        {
            logger.LogError("Comment with id {commentId} does not exist", commentId);
            transactionScope.Rollback();
            return Error.NotFound("comment.update", "comment not found");
        }

        try
        {
            commentUpdates.ApplyTo(comment);
        }
        catch (Exception e)
        {
            logger.LogError(e, "validation error while updating comment");
            transactionScope.Rollback();
            return Error.Validation("comment.update", e.Message);
        }

        comment.UpdatedAt = DateTime.UtcNow.ToUniversalTime();

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while updating comment");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        logger.LogDebug("Updated comment with id {commentId}", commentId);

        return comment.Id.Value;
    }
}