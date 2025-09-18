using System.Data;
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
    private readonly ITransactionManager _transactionManager;

    public UpdateCommentHandler(
        ICommentsRepository commentsRepository,
        ILogger<UpdateCommentHandler> logger,
        ITransactionManager transactionManager)
    {
        _commentsRepository = commentsRepository;
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public async Task<Result<Guid?, Error>> Handle(
        Guid commentId,
        JsonPatchDocument<Comment> commentUpdates,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handlerName}", nameof(UpdateCommentHandler));

        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while creating product");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        Comment? comment = await _commentsRepository.GetCommentById(commentId, cancellationToken);

        if (comment is null)
        {
            _logger.LogError("Comment with id {commentId} does not exist", commentId);
            transactionScope.Rollback();
            return Error.NotFound("comment.update", "comment not found");
        }

        try
        {
            commentUpdates.ApplyTo(comment);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "validation error while updating comment");
            transactionScope.Rollback();
            return Error.Validation("comment.update", e.Message);
        }

        comment.UpdatedAt = DateTime.UtcNow.ToUniversalTime();

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while updating comment");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        _logger.LogDebug("Updated comment with id {commentId}", commentId);

        return comment.Id.Value;
    }
}