using FluentValidation;
using TeaShop.Contract.Comments;

namespace TeaShop.Application.Comments;

public class CreateCommentValidator: AbstractValidator<CreateCommentDto>
{
    public CreateCommentValidator()
    {
        this.RuleFor(c => c.Text).NotEmpty().WithMessage("Текст комментария не может быть пустым");
        this.RuleFor(c => c.UserId).NotEmpty().WithMessage("Идентификатор пользователя не может быть пустым");
        this.RuleFor(c => c.ReviewId).NotEmpty().WithMessage("Идентификатор обзора не может быть пустым");
    }
}