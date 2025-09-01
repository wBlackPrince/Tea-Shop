using CSharpFunctionalExtensions;
using FluentValidation;
using Tea_Shop.Contract.Comments;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Comments.Commands;

public class CreateCommentHandler
{
    private readonly ICommentsRepository _commentsRepository;
    private readonly IValidator<CreateCommentRequestDto> _validator;

    public CreateCommentHandler(
        ICommentsRepository commentsRepository,
        IValidator<CreateCommentRequestDto> validator)
    {
        _commentsRepository = commentsRepository;
        _validator = validator;
    }

    public async Task<Result<Guid?, Error>> Handle(
        CreateCommentRequestDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Error.Validation(
                "create.comment",
                "dto request for create comment is not valid");
        }

        var comment = new Comment(
            new CommentId(Guid.NewGuid()),
            new UserId(request.UserId),
            new ReviewId(request.ReviewId),
            request.Text,
            DateTime.UtcNow,
            DateTime.UtcNow,
            new CommentId(request.ParentId));

        await _commentsRepository.CreateComment(comment, cancellationToken);

        await _commentsRepository.SaveChangesAsync(cancellationToken);

        return comment.Id.Value;
    }
}