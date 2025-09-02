using FluentValidation;
using Tea_Shop.Contract.Reviews;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Reviews.Commands.CreateReviewCommand;

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

        this.RuleFor(r => r.Title)
            .NotNull().WithMessage("Title is required")
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(Constants.Limit50).WithMessage("Title must not exceed 100 characters");

        this.RuleFor(r => r.Text)
            .NotNull().WithMessage("Text is required")
            .NotEmpty().WithMessage("Text is required")
            .MaximumLength(Constants.Limit2000).WithMessage("Text must not exceed 100 characters");
    }
}