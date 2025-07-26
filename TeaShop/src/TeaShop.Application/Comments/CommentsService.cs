using FluentValidation;
using Microsoft.Extensions.Logging;
using TeaShop.Application.Products;
using TeaShop.Contract.Comments;
using TeaShopDomain.Comments;

namespace TeaShop.Application.Comments;

public class CommentsService: ICommentsService
{
    private readonly ICommentsRepository _commentsRepository;
    private readonly ILogger<ProductService> _logger;
    private readonly IValidator<CreateCommentDto> _createCommentValidator;

    public CommentsService(
        ICommentsRepository commentsRepository,
        ILogger<ProductService> logger,
        IValidator<CreateCommentDto> createCommentValidator)
    {
        _commentsRepository = commentsRepository;
        _logger = logger;
        _createCommentValidator = createCommentValidator;
    }


    public async Task<Guid> Create(
        Guid parentId,
        CreateCommentDto request,
        CancellationToken cancellationToken)
    {
        var result = await _createCommentValidator.ValidateAsync(request, cancellationToken);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }

        Guid id = Guid.NewGuid();


        Comment comment = new Comment(
            id,
            request.UserId,
            request.ReviewId,
            parentId,
            0);

        await _commentsRepository.SaveAsync(comment, cancellationToken);

        _logger.LogInformation($"Comment {id} created to comment {parentId}.");

        return id;
    }
}