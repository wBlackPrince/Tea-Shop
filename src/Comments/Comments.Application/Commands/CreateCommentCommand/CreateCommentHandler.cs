using System.Data;
using Comments.Contracts;
using Comments.Contracts.Dtos;
using Comments.Domain;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Abstractions;
using Shared.Database;
using Shared.ValueObjects;

namespace Comments.Application.Commands.CreateCommentCommand;

public class CreateCommentHandler(
    ICommentsRepository commentsRepository,
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
        var review = await commentsRepository.GetReviewById(reviewId, cancellationToken);

        if (review is null)
        {
            logger.LogError($"Review not found with id {reviewId.Value}");
            transactionScope.Rollback();
            return Error.Failure(
                "create.comment",
                $"No review with id {reviewId.Value} found");
        }

        var comment = new Comment(
            new CommentId(Guid.NewGuid()),
            new UserId(command.Request.UserId),
            reviewId,
            command.Request.Text,
            DateTime.UtcNow,
            DateTime.UtcNow,
            new CommentId(command.Request.ParentId));

        await commentsRepository.CreateComment(comment, cancellationToken);

        await transactionManager.SaveChangesAsync(cancellationToken);

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