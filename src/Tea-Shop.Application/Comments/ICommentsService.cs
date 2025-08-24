using Tea_Shop.Contract.Comments;

namespace Tea_Shop.Application.Comments;

public interface ICommentsService
{
    Task<Guid> CreateComment(
        CreateCommentRequestDto request,
        CancellationToken cancellationToken);
}