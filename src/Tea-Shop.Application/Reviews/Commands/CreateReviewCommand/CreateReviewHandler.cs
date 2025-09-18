using System.Data;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Application.Products;
using Tea_Shop.Contract.Reviews;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Reviews.Commands.CreateReviewCommand;

public class CreateReviewHandler: ICommandHandler<CreateReviewResponseDto, CreateReviewCommand>
{
    private readonly IReviewsRepository _reviewsRepository;
    private readonly IProductsRepository _productsRepository;
    private readonly IValidator<CreateReviewRequestDto> _validator;
    private readonly ILogger<CreateReviewHandler> _logger;
    private readonly ITransactionManager _transactionManager;

    public CreateReviewHandler(
        IReviewsRepository reviewsRepository,
        IProductsRepository productsRepository,
        IValidator<CreateReviewRequestDto> validator,
        ILogger<CreateReviewHandler> logger,
        ITransactionManager transactionManager)
    {
        _reviewsRepository = reviewsRepository;
        _productsRepository = productsRepository;
        _validator = validator;
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public async Task<Result<CreateReviewResponseDto, Error>> Handle(
        CreateReviewCommand command,
        CancellationToken cancellationToken)
    {
        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while creating review");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        // валидация входных параметров
        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);

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
        Product? product = await _productsRepository.GetProductById(review.ProductId.Value, cancellationToken);

        if (product is null)
        {
            transactionScope.Rollback();
            return Error.Validation("create.review", "Product not found");
        }

        product.SumRatings += command.Request.ProductRate;
        product.CountRatings += 1;


        await _reviewsRepository.CreateReview(review, cancellationToken);



        await _transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while creating review");
            transactionScope.Rollback();
            return commitedResult.Error;
        }


        _logger.LogDebug("Created review {review.Id}", review.Id);

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