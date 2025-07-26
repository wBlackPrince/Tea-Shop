using FluentValidation;
using Microsoft.Extensions.Logging;
using TeaShop.Contract.Reviews;
using TeaShopDomain.Reviews;

namespace TeaShop.Application.Reviews;

public class ReviewsService: IReviewsService
{
    private readonly IReviewsRepository _reviewsRepository;
    private readonly ILogger<ReviewsService> _logger;
    private readonly IValidator<CreateReviewDto> _createReviewValidator;

    public ReviewsService(
        IReviewsRepository reviewsRepository,
        ILogger<ReviewsService> logger,
        IValidator<CreateReviewDto> createReviewValidator)
    {
        _reviewsRepository = reviewsRepository;
        _createReviewValidator = createReviewValidator;
        _logger = logger;
    }

    public async Task<Guid> Create(CreateReviewDto request, CancellationToken cancellationToken)
    {
        var result = _createReviewValidator.Validate(request);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }

        Guid id = Guid.NewGuid();
        Review review = new Review(
            id,
            request.Title,
            0,
            request.UserId);

        await _reviewsRepository.AddAsync(review, cancellationToken);

        _logger.LogInformation($"Created review {review.Id}");

        return id;
    }
}