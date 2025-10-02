using FluentValidation;
using Reviews.Contracts;
using Reviews.Contracts.Dtos;
using Shared;

namespace Reviews.Application.Commands.CreateReviewCommand;

public class CreateReviewValidator:AbstractValidator<CreateReviewRequestDto>
{
    public CreateReviewValidator()
    {
        this.RuleFor(r => r.ProductId)
            .NotNull().WithMessage("Product Id is required")
            .NotEmpty().WithMessage("Product Id is required");

        this.RuleFor(r => r.UserId)
            .NotNull().WithMessage("User Id is required")
            .NotEmpty().WithMessage("User Id is required");

        this.RuleFor(r => r.ProductRate)
            .NotNull().WithMessage("Product rate is required")
            .NotEmpty().WithMessage("Product rate is required")
            .Must(BeValidProductRate).WithMessage("Product rate must be valid");

        this.RuleFor(r => r.Title)
            .NotNull().WithMessage("Title is required")
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(Constants.Limit50).WithMessage("Title must not exceed 100 characters");

        this.RuleFor(r => r.Text)
            .NotNull().WithMessage("Text is required")
            .NotEmpty().WithMessage("Text is required")
            .MaximumLength(Constants.Limit2000).WithMessage("Text must not exceed 100 characters");
    }

    private bool BeValidProductRate(int rate)
    {
        return (rate >= 1) && (rate <= 5);
    }
}