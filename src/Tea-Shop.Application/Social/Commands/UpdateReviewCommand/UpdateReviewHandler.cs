using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Domain.Social;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Social.Commands.UpdateReviewCommand;

public class UpdateReviewHandler(
    ISocialRepository reviewsRepository,
    ILogger<UpdateReviewHandler> logger,
    ITransactionManager transactionManager): ICommandHandler<Guid, UpdateReviewCommand>
{
    public async Task<Result<Guid, Error>> Handle(
        UpdateReviewCommand command,
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
            new ReviewId(command.ReviewId),
            cancellationToken);

        if (review is null)
        {
            transactionScope.Rollback();
            return Error.NotFound("update review", "review not found");
        }

        UnitResult<Error> result = new UnitResult<Error>();

        switch (command.Request.Property)
        {
            case nameof(review.Title):
                result = review.UpdateTitle(command.Request.NewValue);
                break;
            case nameof(review.Text):
                result = review.UpdateText(command.Request.NewValue);
                break;
            case nameof(review.Rating):
                result = review.UpdateRating(int.Parse(command.Request.NewValue));
                break;
            default:
                logger.LogError("Validation error while updating review with id {reviewId}", command.ReviewId);
                transactionScope.Rollback();
                return Error.Validation("update review", "invalid property to update");
        }

        if (result.IsFailure)
        {
            logger.LogError("fail to update comment");
            transactionScope.Rollback();
            return result.Error;
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


        logger.LogInformation("Update review {reviewId}", command.ReviewId);

        return review.Id.Value;
    }
}