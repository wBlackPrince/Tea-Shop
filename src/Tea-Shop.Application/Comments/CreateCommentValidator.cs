using FluentValidation;
using Tea_Shop.Contract.Comments;
using Tea_Shop.Shared;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public class CreateCommentValidator: AbstractValidator<CreateCommentRequestDto>
{
    public CreateCommentValidator()
    {
        this.RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Text cannot be empty")
            .NotNull().WithMessage("Text cannot be null")
            .MaximumLength(Constants.Limit2000).WithMessage("Text cannot be longer than 2000 characters");
    }
}