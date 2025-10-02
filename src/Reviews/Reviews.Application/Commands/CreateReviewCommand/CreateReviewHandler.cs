using System.Data;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Reviews.Contracts;
using Reviews.Contracts.Dtos;
using Reviews.Domain;
using Shared;
using Shared.Abstractions;
using Shared.Database;
using Shared.ValueObjects;

namespace Reviews.Application.Commands.CreateReviewCommand;

public class CreateReviewHandler(
    IReviewsRepository reviewsRepository,
    IProductsRepository productsRepository,
    IValidator<CreateReviewRequestDto> validator,
    ILogger<CreateReviewHandler> logger,
    ITransactionManager transactionManager): ICommandHandler<CreateReviewResponseDto, CreateReviewCommand>
{
    public async Task<Result<CreateReviewResponseDto, Error>> Handle(
        CreateReviewCommand command,
        CancellationToken cancellationToken)
    {
        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while creating review");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        // валидация входных параметров
        var validationResult = await validator.ValidateAsync(command.Request, cancellationToken);

        if (!validationResult.IsValid)
        {
            transactionScope.Rollback();
            return Error.Validation("create.review", "Validation Failed");
        }

        Review review = new Review(
            new ReviewId(Guid.NewGuid()),
            new ProductId(command.Request.ProductId),
            new UserId(command.Request.UserId),
            command.Request.ProductRate,
            command.Request.Title,
            command.Request.Text,
            DateTime.UtcNow,
            DateTime.UtcNow);


        // обновляем количество отзывов и сумму рейтинга у продукта
        var product = await productsRepository.GetProductById(review.ProductId.Value, cancellationToken);

        if (product is null)
        {
            transactionScope.Rollback();
            return Error.Validation("create.review", "Product not found");
        }

        product.SumRatings += command.Request.ProductRate;
        product.CountRatings += 1;


        await reviewsRepository.CreateReview(review, cancellationToken);



        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while creating review");
            transactionScope.Rollback();
            return commitedResult.Error;
        }


        logger.LogDebug("Created review {review.Id}", review.Id);

        var response = new CreateReviewResponseDto
        {
            Id = review.Id.Value,
            UserId = review.UserId.Value,
            ProductRate = (int)review.ProductRating,
            Title = review.Title,
            Text = review.Text,
        };

        return response;
    }
}