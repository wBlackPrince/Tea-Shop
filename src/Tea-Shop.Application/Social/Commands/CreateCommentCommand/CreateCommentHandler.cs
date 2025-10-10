using System.Data;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Social;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Social.Commands.CreateCommentCommand;

public class CreateCommentHandler(
    ISocialRepository socialRepository,
    IValidator<CreateCommentRequestDto> validator,
    ILogger<CreateCommentHandler> logger,
    ITransactionManager transactionManager
    ): ICommandHandler<CreateCommentResponseDto, CreateCommentCommand>
{
    public async Task<Result<CreateCommentResponseDto, Error>> Handle(
        CreateCommentCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handlerName}", nameof(CreateCommentHandler));

        var validationResult = await validator.ValidateAsync(command.Request, cancellationToken);

        if (!validationResult.IsValid)
        {
            logger.LogError($"Dto request for creating comment is not valid");
            return Error.Validation(
                "create.comment",
                "dto request for create comment is not valid");
        }


        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while creating product");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        ReviewId reviewId = new ReviewId(command.Request.ReviewId);
        var review = await socialRepository.GetReviewById(reviewId, cancellationToken);

        if (review is null)
        {
            logger.LogError($"Review not found with id {reviewId.Value}");
            transactionScope.Rollback();
            return Error.Failure(
                "create.comment",
                $"No review with id {reviewId.Value} found");
        }


        var commentId = new CommentId(Guid.NewGuid());
        var userId = new UserId(command.Request.UserId);
        Comment? comment;

        if (command.Request.ParentId is not null)
        {
            var parentId = new CommentId(command.Request.ParentId);
            var parentComment = await socialRepository.GetCommentById(
                parentId.Value.Value,
                cancellationToken);

            if (parentComment is null)
            {
                logger.LogError($"Review not found with id {reviewId.Value}");
                transactionScope.Rollback();
                return Error.NotFound(
                    "create.comment",
                    $"No comment with id {parentId.Value} found");
            }

            var path = parentComment.Path.CreateChild(new Identifier(commentId.Value.ToString()));

            comment = Comment.CreateChild(
                userId,
                reviewId,
                command.Request.Text,
                DateTime.UtcNow,
                DateTime.UtcNow,
                new Identifier(commentId.Value.ToString()),
                parentComment,
                parentId,
                commentId);
        }
        else
        {
            comment = Comment.CreateParent(
                userId,
                reviewId,
                command.Request.Text,
                DateTime.UtcNow,
                DateTime.UtcNow,
                new Identifier(commentId.Value.ToString()),
                commentId);
        }

        await socialRepository.CreateComment(comment, cancellationToken);

        var savingResult = await transactionManager.SaveChangesAsync(cancellationToken);

        if (savingResult.IsFailure)
        {
            logger.LogError("Failed to save changes while creating comment");
            transactionScope.Rollback();
            return savingResult.Error;
        }

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while creating comment");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        logger.LogDebug($"Comment created: {comment.Id}");


        var response = new CreateCommentResponseDto()
        {
            Id = comment.Id.Value,
            UserId = comment.UserId.Value,
            Text = comment.Text,
            ReviewId = comment.ReviewId.Value,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            ParentId = comment?.ParentId?.Value,
        };

        return response;
    }
}