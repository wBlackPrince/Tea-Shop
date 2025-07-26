using TeaShopDomain.Comments;

namespace TeaShop.Application.Comments;

public interface ICommentsRepository
{
    Task<Guid> AddAsync(Comment comment, CancellationToken cancellationToken);

    Task<Guid> SaveAsync(Comment comment, CancellationToken cancellationToken);

    Task<Guid> DeleteAsync(Comment commentId, CancellationToken cancellationToken);
}