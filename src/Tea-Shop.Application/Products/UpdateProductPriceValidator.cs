using FluentValidation;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products;

public class UpdateProductPriceValidator: AbstractValidator<UpdateProductPriceRequestDto>
{
    public UpdateProductPriceValidator()
    {
        this.RuleFor(p => p.Price)
            .NotEmpty().WithMessage("Price is required")
            .NotNull().WithMessage("Price is required")
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}