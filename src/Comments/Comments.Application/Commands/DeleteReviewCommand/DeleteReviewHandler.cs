using System.Data;
using Comments.Contracts.Dtos;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Abstractions;
using Shared.Database;
using Shared.ValueObjects;

namespace Comments.Application.Commands.DeleteReviewCommand;

public class DeleteReviewHandler(
    IReviewsReadDbContext readDbContext,
    IReviewsRepository reviewsRepository,
    ILogger<DeleteReviewHandler> logger,
    ITransactionManager transactionManager):
    ICommandHandler<DeleteReviewDto, DeleteReviewCommand>
{
    public async Task<Result<DeleteReviewDto, Error>> Handle(
        DeleteReviewCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(DeleteReviewHandler));



        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while deleting review");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;



        var review = await readDbContext.ReviewsRead
            .FirstOrDefaultAsync(
                r => r.Id == new ReviewId(command.Request.ReviewId),
                cancellationToken);

        if (review is null)
        {
            logger.LogWarning("Review with id {reviewId} not found", command.Request.ReviewId);
            transactionScope.Rollback();
            return Error.NotFound("delete.review", "review not found");
        }

        await reviewsRepository.DeleteReview(
            new ReviewId(command.Request.ReviewId),
            cancellationToken);


        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while deleting review");
            transactionScope.Rollback();
            return commitedResult.Error;
        }


        logger.LogDebug("Deleted review with id {reviewId}", command.Request.ReviewId);

        return command.Request;
    }
}