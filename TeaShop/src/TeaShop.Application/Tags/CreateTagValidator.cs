using FluentValidation;
using TeaShop.Contract.Tags;

namespace TeaShop.Application.Tags;

public class CreateTagValidator: AbstractValidator<CreateTagDto>
{
    public CreateTagValidator()
    {
        this.RuleFor(t => t.Name).NotEmpty().WithMessage("Имя тега не может быть пустым");
        this.RuleFor(t => t.Description).NotEmpty().WithMessage("Описание тега не может быть пустым");
    }
}