using FluentValidation;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Commands.UpdateProductIngredients;

public class UpdateProductIngridientsValidator: AbstractValidator<UpdateProductIngredientsCommand>
{
    public UpdateProductIngridientsValidator()
    {
        this.RuleForEach(p => p.IngridientsDto.Ingridients)
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
    }
}