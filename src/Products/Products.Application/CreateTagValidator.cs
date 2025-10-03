using FluentValidation;
using Products.Contracts.Dtos;
using Shared;

namespace Products.Application;

public class CreateTagValidator: AbstractValidator<CreateTagRequestDto>
{
    public CreateTagValidator()
    {
        this
            .RuleFor(t => t.Name)
            .NotNull().WithMessage("Name is required")
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(Constants.Limit50).WithMessage("Name cannot exceed 50 characters");

        this
            .RuleFor(t => t.Description)
            .NotNull().WithMessage("Description is required")
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(Constants.Limit500).WithMessage("Description cannot exceed 500 characters");
    }
}