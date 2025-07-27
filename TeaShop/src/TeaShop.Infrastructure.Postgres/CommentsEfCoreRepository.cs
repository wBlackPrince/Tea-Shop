using TeaShop.Application.Comments;
using TeaShopDomain.Comments;

namespace TeaShop.Infrastructure.Postgres;

public class CommentsEfCoreRepository: ICommentsRepository
{
    public async Task<Guid> AddAsync(Comment comment, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> SaveAsync(Comment comment, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> DeleteAsync(Comment commentId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}