using FluentValidation;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Commands.CreateProductCommand;

public class CreateProductValidator: AbstractValidator<CreateProductRequestDto>
{
    public CreateProductValidator()
    {
        this.RuleFor(p => p.Title)
            .NotEmpty().WithMessage("Title is required")
            .NotNull().WithMessage("Title is required")
            .MaximumLength(Constants.Limit100).WithMessage("Title must not exceed 100 characters")
            .MinimumLength(Constants.Limit2).WithMessage("Title must contain at least 2 character");

        this.RuleFor(p => p.Description)
            .NotEmpty().WithMessage("Description is required")
            .NotNull().WithMessage("Description is required")
            .MaximumLength(Constants.Limit2000).WithMessage("Description must not exceed 2000 characters")
            .MinimumLength(Constants.Limit2).WithMessage("Description must contain at least 2 character");

        this.RuleFor(p => p.Season)
            .NotEmpty().WithMessage("Season is required")
            .NotNull().WithMessage("Season is required")
            .Must(BeValidSeason).WithMessage("Season is invalid. Enter SUMMER, AUTUMN, WINTER or SPRING");

        this.RuleFor(p => p.Price)
            .NotEmpty().WithMessage("Price is required")
            .NotNull().WithMessage("Price is required")
            .GreaterThan(0).WithMessage("Price must be greater than zero");

        this.RuleFor(p => p.Amount)
            .NotEmpty().WithMessage("Amount is required")
            .NotNull().WithMessage("Amount is required")
            .GreaterThan(0).WithMessage("Amount must be greater than zero");

        this.RuleFor(p => p.StockQuantity)
            .NotEmpty().WithMessage("Stock quantity is required")
            .NotNull().WithMessage("Stock quantity is required")
            .GreaterThan(0).WithMessage("Stock quantity must be greater than zero");

        this.RuleFor(p => p.Ingridients)
            .NotEmpty().WithMessage("Ingredients is required")
            .NotNull().WithMessage("Ingredients is required");

        this.RuleForEach(p => p.Ingridients)
            .ChildRules(ingrindients =>
            {
                ingrindients.RuleFor(i => i.Name)
                    .NotEmpty().WithMessage("Name is required")
                    .NotNull().WithMessage("Name is required")
                    .MaximumLength(Constants.Limit50).WithMessage("Name must not exceed 50 characters")
                    .MinimumLength(Constants.Limit2).WithMessage("Name must contain at least 2 character");
                ingrindients.RuleFor(i => i.Amount)
                    .NotEmpty().WithMessage("Amount is required")
                    .GreaterThan(0).WithMessage("Amount must be greater than zero");
                ingrindients.RuleFor(i => i.Description)
                    .NotEmpty().WithMessage("Description is required")
                    .MaximumLength(Constants.Limit1000).WithMessage("Description must not exceed 1000 characters")
                    .MinimumLength(Constants.Limit2).WithMessage("Description must contain at least 2 character");
                ingrindients.RuleFor(i => i.IsAllergen)
                    .NotNull().WithMessage("IsAllergen is required");
            });

        this.RuleFor(p => p.PreparationDescription)
            .NotEmpty().WithMessage("Preparation Method is required")
            .NotNull().WithMessage("Preparation Method is required")
            .MaximumLength(Constants.Limit1000).WithMessage("Preparation Method must not exceed 1000 characters")
            .MinimumLength(Constants.Limit2).WithMessage("Preparation Method must contain at least 2 character");

        this.RuleFor(p => p.PreparationTime)
            .NotEmpty().WithMessage("Preparation Time is required")
            .NotNull().WithMessage("Preparation Time is required")
            .GreaterThan(0);

        this.RuleFor(p => p.TagsIds)
            .NotEmpty().WithMessage("At least 1 tag is required")
            .NotNull().WithMessage("Tags is required");
    }

    private bool BeValidSeason(string season)
    {
        return Enum.TryParse(typeof(Season), season, out _);
    }
}