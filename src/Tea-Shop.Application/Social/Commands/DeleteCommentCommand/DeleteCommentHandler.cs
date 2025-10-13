using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Social;
using Tea_Shop.Domain.Social;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Social.Commands.DeleteCommentCommand;

public class DeleteCommentHandler(
    ISocialRepository socialRepository,
    IReadDbContext readDbContext,
    ILogger<DeleteCommentHandler> logger,
    ITransactionManager transactionManager):
    ICommandHandler<CommentWithOnlyIdDto, DeleteCommentCommand>
{
    public async Task<Result<CommentWithOnlyIdDto, Error>> Handle(
        DeleteCommentCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(DeleteCommentHandler));

        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while creating product");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;

        var comment = await readDbContext.CommentsRead.FirstOrDefaultAsync(
            c => c.Id == new CommentId(command.Request.CommentId),
            cancellationToken);

        if (comment is null)
        {
            logger.LogError("Comment with id {commentId} not flund", command.Request.CommentId);
            transactionScope.Rollback();
            return Error.Failure("delete.comment", "Comment not found");
        }

        await socialRepository.DeleteComment(new CommentId(command.Request.CommentId), cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while deleting comment");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        logger.LogDebug("Deleted comment with id {commentId}", command.Request.CommentId);

        return command.Request;
    }
}