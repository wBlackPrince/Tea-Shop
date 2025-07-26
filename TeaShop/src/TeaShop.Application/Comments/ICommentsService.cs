using TeaShop.Contract.Comments;

namespace TeaShop.Application.Comments;

public interface ICommentsService
{
    Task<Guid> Create(Guid parentId, CreateCommentDto request, CancellationToken cancellationToken);
}