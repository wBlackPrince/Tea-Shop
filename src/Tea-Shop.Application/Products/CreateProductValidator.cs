using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;

namespace Tea_Shop.Application;

public class CreateProductValidator: AbstractValidator<CreateProductRequestDto>
{
    public CreateProductValidator()
    {
        this.RuleFor(p => p.Title)
            .NotEmpty().WithMessage("Title is required")
            .NotNull().WithMessage("Title is required")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters")
            .MinimumLength(1).WithMessage("Title must contain at least 1 character");

        this.RuleFor(p => p.Price)
            .NotEmpty().WithMessage("Price is required")
            .NotNull().WithMessage("Price is required")
            .GreaterThan(0).WithMessage("Price must be greater than zero");

        this.RuleFor(p => p.Amount)
            .NotEmpty().WithMessage("Amount is required")
            .NotNull().WithMessage("Amount is required")
            .GreaterThan(0).WithMessage("Amount must be greater than zero");

        this.RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Description is required")
            .NotNull().WithMessage("Description is required")
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        this.RuleFor(p => p.Season)
            .NotEmpty().WithMessage("Season is required")
            .NotNull().WithMessage("Season is required")
            .Must(BeValidSeason).WithMessage("Season is invalid. Enter SUMMER, AUTUMN, WINTER or SPRING");

        this.RuleFor(p => p.Ingridients)
            .NotEmpty().WithMessage("Ingredients is required")
            .NotNull().WithMessage("Ingredients is required");

        this.RuleForEach(p => p.Ingridients)
            .ChildRules(ingrindients =>
            {
                ingrindients.RuleFor(i => i.Name)
                    .NotEmpty().WithMessage("Name is required")
                    .NotNull().WithMessage("Name is required");
                ingrindients.RuleFor(i => i.Amount)
                    .NotEmpty().WithMessage("Amount is required")
                    .GreaterThan(0).WithMessage("Amount must be greater than zero");
                ingrindients.RuleFor(i => i.Description)
                    .NotEmpty().WithMessage("Description is required")
                    .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters")
                    .MinimumLength(5).WithMessage("Description must contain at least 5 character");
                ingrindients.RuleFor(i => i.IsAllergen)
                    .NotEmpty().WithMessage("IsAllergen is required")
                    .NotNull().WithMessage("IsAllergen is required");
            });

        this.RuleFor(p => p.PreparationmMethod)
            .NotEmpty().WithMessage("PreparationmMethod is required")
            .NotNull().WithMessage("PreparationmMethod is required")
            .MaximumLength(400).WithMessage("PreparationmMethod must not exceed 400 characters")
            .MinimumLength(5).WithMessage("PreparationmMethod must contain at least 5 character");

        this.RuleFor(p => p.TagsIds)
            .NotEmpty().WithMessage("At least 1 tag is required")
            .NotNull().WithMessage("Tags is required");
    }

    private bool BeValidSeason(string season)
    {
        return Enum.TryParse(typeof(Season), season, out _);
    }
}