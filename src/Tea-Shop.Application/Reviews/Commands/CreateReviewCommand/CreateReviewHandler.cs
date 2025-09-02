using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Reviews;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Reviews.Commands.CreateReviewCommand;

public class CreateReviewHandler: ICommandHandler<CreateReviewResponseDto, CreateReviewCommand>
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

    public async Task<Result<CreateReviewResponseDto, Error>> Handle(
        CreateReviewCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Error.Validation("create.review", "Validation Failed");
        }

        Review review = new Review(
            new ReviewId(Guid.NewGuid()),
            new ProductId(command.Request.ProductId),
            new UserId(command.Request.UserId),
            command.Request.Title,
            command.Request.Text,
            DateTime.UtcNow,
            DateTime.UtcNow);

        await _reviewsRepository.CreateReview(review, cancellationToken);

        await _reviewsRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created review {review.Id}", review.Id);

        var response = new CreateReviewResponseDto
        {
            Id = review.Id.Value,
            UserId = review.UserId.Value,
            Title = review.Title,
            Text = review.Text
        };

        return response;
    }
}