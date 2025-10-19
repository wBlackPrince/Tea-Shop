using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract;
using Tea_Shop.Domain.Social;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Social.Commands.UpdateCommentCommand;

public class UpdateCommentHandler(
    ISocialRepository socialRepository,
    ILogger<UpdateCommentHandler> logger,
    ITransactionManager transactionManager): ICommandHandler<Guid?, UpdateCommentCommand>
{
    public async Task<Result<Guid?, Error>> Handle(
        UpdateCommentCommand command,
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


        Comment? comment = await socialRepository.GetCommentById(command.CommentId, cancellationToken);

        if (comment is null)
        {
            logger.LogError("Comment with id {commentId} does not exist", command.CommentId);
            transactionScope.Rollback();
            return Error.NotFound("comment.update", "comment not found");
        }

        UnitResult<Error> result = new UnitResult<Error>();

        switch (command.Request.Property)
        {
            case nameof(comment.Text):
                result = comment.UpdateText(command.Request.NewValue);
                break;
            case nameof(comment.Rating):
                result = comment.UpdateRating(int.Parse(command.Request.NewValue));
                break;
            default:
                logger.LogError("validation error while updating comment");
                transactionScope.Rollback();
                return Error.Validation("comment.update", "invalid property to update");
        }

        if (result.IsFailure)
        {
            logger.LogError("fail to update comment");
            transactionScope.Rollback();
            return result.Error;
        }


        comment.UpdatedAt = DateTime.UtcNow.ToUniversalTime();

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while updating comment");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        logger.LogDebug("Updated comment with id {commentId}", command.CommentId);

        return comment.Id.Value;
    }
}