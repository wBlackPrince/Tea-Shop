using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Contract.Products;
using Tea_Shop.Contract.Reviews;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Application.Reviews;

public class ReviewsService : IReviewsService
{
    private readonly IReviewsRepository _reviewsRepository;
    private readonly IValidator<CreateReviewRequestDto> _createReviewValidator;
    private readonly ILogger<ReviewsService> _logger;


    public ReviewsService(
        IReviewsRepository reviewsRepository,
        IValidator<CreateReviewRequestDto> createReviewValidator,
        ILogger<ReviewsService> logger)
    {
        _reviewsRepository = reviewsRepository;
        _createReviewValidator = createReviewValidator;
        _logger = logger;
    }


    public async Task<Guid> CreateReview(
        CreateReviewRequestDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _createReviewValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        Review review = new Review(
            new ReviewId(Guid.NewGuid()),
            new ProductId(request.ProductId),
            new UserId(request.UserId),
            request.Title,
            request.Text,
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        await _reviewsRepository.CreateReview(review, cancellationToken);

        await _reviewsRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created review {review.Id}", review.Id);

        return review.Id.Value;
    }
}