using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Contract.Reviews;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Reviews.Commands;

public class CreateReviewHandler
{
    private readonly IReviewsRepository _reviewsRepository;
    private readonly IValidator<CreateReviewRequestDto> _validator;
    private readonly ILogger<CreateReviewHandler> _logger;

    public CreateReviewHandler(
        IReviewsRepository reviewsRepository,
        IValidator<CreateReviewRequestDto> validator,
        ILogger<CreateReviewHandler> logger)
    {
        _reviewsRepository = reviewsRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateReviewRequestDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Error.Validation("create.review", "Validation Failed");
        }

        Review review = new Review(
            new ReviewId(Guid.NewGuid()),
            new ProductId(request.ProductId),
            new UserId(request.UserId),
            request.Title,
            request.Text,
            DateTime.UtcNow,
            DateTime.UtcNow);

        await _reviewsRepository.CreateReview(review, cancellationToken);

        await _reviewsRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created review {review.Id}", review.Id);

        return review.Id.Value;
    }
}