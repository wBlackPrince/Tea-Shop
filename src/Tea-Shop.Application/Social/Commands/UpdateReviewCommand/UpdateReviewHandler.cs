using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Database;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Social.Commands.UpdateReviewCommand;

public class UpdateReviewHandler(
    ISocialRepository reviewsRepository,
    ILogger<UpdateReviewHandler> logger,
    ITransactionManager transactionManager)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid reviewId,
        JsonPatchDocument<Review> reviewUpdates,
        CancellationToken cancellationToken)
    {
        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while updating review");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        Review? review = await reviewsRepository.GetReviewById(
            new ReviewId(reviewId),
            cancellationToken);

        if (review is null)
        {
            transactionScope.Rollback();
            return Error.NotFound("update review", "review not found");
        }

        try
        {
            reviewUpdates.ApplyTo(review);
        }
        catch (Exception e)
        {
            logger.LogError("Validation error while updating review with id {reviewId}", reviewId);
            transactionScope.Rollback();
            return Error.Validation("update review", e.Message);
        }

        review.UpdatedAt = DateTime.UtcNow.ToUniversalTime();


        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while updating review");
            transactionScope.Rollback();
            return commitedResult.Error;
        }


        logger.LogInformation("Update review {reviewId}", reviewId);

        return review.Id.Value;
    }
}