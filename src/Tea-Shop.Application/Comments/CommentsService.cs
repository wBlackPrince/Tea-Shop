using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Contract.Comments;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public class CommentsService : ICommentsService
{
    private readonly ICommentsRepository _commentsRepository;
    private readonly ILogger<CommentsService> _logger;
    private readonly IValidator<CreateCommentRequestDto> _createCommentValidator;

    public CommentsService(
        ICommentsRepository commentsRepository,
        ILogger<CommentsService> logger,
        IValidator<CreateCommentRequestDto> createCommentValidator)
    {
        _commentsRepository = commentsRepository;
        _logger = logger;
        _createCommentValidator = createCommentValidator;
    }

    public async Task<Guid> CreateComment(
        CreateCommentRequestDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _createCommentValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        Comment comment = new Comment(
            new CommentId(Guid.NewGuid()),
            new UserId(request.UserId),
            new ReviewId(request.ReviewId),
            request.Text,
            DateTime.UtcNow,
            DateTime.UtcNow);

        await _commentsRepository.CreateComment(comment, cancellationToken);

        await _commentsRepository.SaveChangesAsync(cancellationToken);

        return comment.Id.Value;
    }
}