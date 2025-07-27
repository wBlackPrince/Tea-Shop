using FluentValidation;
using TeaShop.Contract.Products;

namespace TeaShop.Application.Products;

public class CreateProductValidator: AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        this.RuleFor(p => p.Title).NotEmpty().WithMessage("Название продукта не может быть пустым");
        this.RuleFor(p => p.Price).NotEmpty().WithMessage("Цена товара не может быть пустым");
        this.RuleFor(p => p.Description).NotEmpty().WithMessage("Описание товара не может быть пустым");
    }
}