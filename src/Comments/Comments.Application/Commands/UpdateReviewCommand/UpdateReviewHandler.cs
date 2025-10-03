using System.ComponentModel.DataAnnotations;
using System.Data;
using Comments.Domain;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Shared;
using Shared.Database;
using Shared.Dto;
using Shared.ValueObjects;

namespace Comments.Application.Commands.UpdateReviewCommand;

public class UpdateReviewHandler(
    ICommentsRepository reviewsRepository,
    ILogger<UpdateReviewHandler> logger,
    ITransactionManager transactionManager)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid reviewId,
        UpdateEntityRequestDto request,
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
            switch (request.Property)
            {
                case nameof(review.Text):
                    review.Text = (string)request.NewValue;
                    break;
                case nameof(review.Rating):
                    review.Rating = (int)request.NewValue;
                    break;
                case nameof(review.Title):
                    review.Title = (string)request.NewValue;
                    break;
                default:
                    throw new ValidationException("Invalid property");
            }
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