using System.Data;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Reviews;
using Tea_Shop.Contract.Comments;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Comments.Commands.CreateCommentCommand;

public class CreateCommentHandler: ICommandHandler<CreateCommentResponseDto, CreateCommentCommand>
{
    private readonly ICommentsRepository _commentsRepository;
    private readonly IReviewsRepository _reviewsRepository;
    private readonly IValidator<CreateCommentRequestDto> _validator;
    private readonly ILogger<CreateCommentHandler> _logger;
    private readonly ITransactionManager _transactionManager;

    public CreateCommentHandler(
        ICommentsRepository commentsRepository,
        IReviewsRepository reviewsRepository,
        IValidator<CreateCommentRequestDto> validator,
        ILogger<CreateCommentHandler> logger,
        ITransactionManager transactionManager)
    {
        _commentsRepository = commentsRepository;
        _reviewsRepository = reviewsRepository;
        _validator = validator;
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public async Task<Result<CreateCommentResponseDto, Error>> Handle(
        CreateCommentCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handlerName}", nameof(CreateCommentHandler));

        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError($"Dto request for creating comment is not valid");
            return Error.Validation(
                "create.comment",
                "dto request for create comment is not valid");
        }


        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while creating product");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        ReviewId reviewId = new ReviewId(command.Request.ReviewId);
        var review = await _reviewsRepository.GetReviewById(reviewId, cancellationToken);

        if (review is null)
        {
            _logger.LogError($"Review not found with id {reviewId.Value}");
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

        await _commentsRepository.CreateComment(comment, cancellationToken);

        await _transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while creating comment");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        _logger.LogDebug($"Comment created: {comment.Id}");


        var response = new CreateCommentResponseDto()
        {
            Id = comment.Id.Value,
            UserId = comment.UserId.Value,
            Text = comment.Text,
            ReviewId = comment.ReviewId.Value,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            ParentId = comment.ParentId.Value,
        };

        return response;
    }
}