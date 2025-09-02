using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Comments;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Comments.Commands.CreateCommentCommand;

public class CreateCommentHandler: ICommandHandler<CreateCommentResponseDto, CreateCommentCommand>
{
    private readonly ICommentsRepository _commentsRepository;
    private readonly IValidator<CreateCommentRequestDto> _validator;
    private readonly ILogger<CreateCommentHandler> _logger;

    public CreateCommentHandler(
        ICommentsRepository commentsRepository,
        IValidator<CreateCommentRequestDto> validator,
        ILogger<CreateCommentHandler> logger)
    {
        _commentsRepository = commentsRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<CreateCommentResponseDto, Error>> Handle(
        CreateCommentCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Error.Validation(
                "create.comment",
                "dto request for create comment is not valid");
        }

        var comment = new Comment(
            new CommentId(Guid.NewGuid()),
            new UserId(command.Request.UserId),
            new ReviewId(command.Request.ReviewId),
            command.Request.Text,
            DateTime.UtcNow,
            DateTime.UtcNow,
            new CommentId(command.Request.ParentId));

        await _commentsRepository.CreateComment(comment, cancellationToken);

        await _commentsRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation($"Comment created: {comment.Id}");


        var response = new CreateCommentResponseDto()
        {
            Id = comment.Id.Value,
            UserId = comment.UserId.Value,
            Text = comment.Text,
            ReviewId = comment.ReviewId.Value,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            ParentId = comment.ParentId.Value
        };

        return response;
    }
}