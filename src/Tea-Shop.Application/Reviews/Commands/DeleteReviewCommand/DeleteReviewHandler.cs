using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Reviews;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Reviews.Commands.DeleteReviewCommand;

public class DeleteReviewHandler:
    ICommandHandler<DeleteReviewDto, DeleteReviewCommand>
{
    private readonly IReadDbContext _readDbContext;
    private readonly IReviewsRepository _reviewsRepository;
    private readonly ILogger<DeleteReviewHandler> _logger;
    private readonly ITransactionManager _transactionManager;

    public DeleteReviewHandler(
        IReadDbContext readDbContext,
        IReviewsRepository reviewsRepository,
        ILogger<DeleteReviewHandler> logger,
        ITransactionManager transactionManager)
    {
        _readDbContext = readDbContext;
        _reviewsRepository = reviewsRepository;
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public async Task<Result<DeleteReviewDto, Error>> Handle(
        DeleteReviewCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handler}", nameof(DeleteReviewHandler));



        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while deleting review");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;



        var review = await _readDbContext.ReviewsRead
            .FirstOrDefaultAsync(
                r => r.Id == new ReviewId(command.Request.ReviewId),
                cancellationToken);

        if (review is null)
        {
            _logger.LogWarning("Review with id {reviewId} not found", command.Request.ReviewId);
            transactionScope.Rollback();
            return Error.NotFound("delete.review", "review not found");
        }

        await _reviewsRepository.DeleteReview(
            new ReviewId(command.Request.ReviewId),
            cancellationToken);


        await _transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while deleting review");
            transactionScope.Rollback();
            return commitedResult.Error;
        }


        _logger.LogDebug("Deleted review with id {reviewId}", command.Request.ReviewId);

        return command.Request;
    }
}