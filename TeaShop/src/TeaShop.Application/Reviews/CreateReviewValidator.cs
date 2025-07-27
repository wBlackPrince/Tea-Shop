using FluentValidation;
using TeaShop.Contract.Reviews;

namespace TeaShop.Application.Reviews;

public class CreateReviewValidator: AbstractValidator<CreateReviewDto>
{
    public CreateReviewValidator()
    {
        this.RuleFor(r => r.Title).NotEmpty().WithMessage("Заголовок обзора не может быть пустым");
        this.RuleFor(r => r.UserId).NotEmpty().WithMessage("Идентификатор пользователя не может быть пустым");
    }
}