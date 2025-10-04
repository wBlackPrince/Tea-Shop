using System.Data;
using Comments.Contracts;
using Comments.Contracts.Dtos;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Abstractions;
using Shared.Database;
using Shared.ValueObjects;

namespace Comments.Application.Commands.DeleteCommentCommand;

public class DeleteCommentHandler(
    ICommentsRepository commentsRepository,
    ICommentsReadDbContext readDbContext,
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

        await commentsRepository.DeleteComment(new CommentId(command.Request.CommentId), cancellationToken);

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