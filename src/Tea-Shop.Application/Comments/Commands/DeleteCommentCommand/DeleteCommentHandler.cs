using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Comments;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Comments.Commands.DeleteCommentCommand;

public class DeleteCommentHandler:
    ICommandHandler<CommentWithOnlyIdDto, DeleteCommentCommand>
{
    private readonly ICommentsRepository _commentsRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<DeleteCommentHandler> _logger;
    private readonly ITransactionManager _transactionManager;

    public DeleteCommentHandler(
        ICommentsRepository commentsRepository,
        IReadDbContext readDbContext,
        ILogger<DeleteCommentHandler> logger,
        ITransactionManager transactionManager)
    {
        _commentsRepository = commentsRepository;
        _readDbContext = readDbContext;
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public async Task<Result<CommentWithOnlyIdDto, Error>> Handle(
        DeleteCommentCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handler}", nameof(DeleteCommentHandler));

        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while creating product");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;

        var comment = await _readDbContext.CommentsRead.FirstOrDefaultAsync(
            c => c.Id == new CommentId(command.Request.CommentId),
            cancellationToken);

        if (comment is null)
        {
            _logger.LogError("Comment with id {commentId} not flund", command.Request.CommentId);
            transactionScope.Rollback();
            return Error.Failure("delete.comment", "Comment not found");
        }

        await _commentsRepository.DeleteComment(new CommentId(command.Request.CommentId), cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while deleting comment");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        _logger.LogDebug("Deleted comment with id {commentId}", command.Request.CommentId);

        return command.Request;
    }
}