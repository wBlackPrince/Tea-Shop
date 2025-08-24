using Tea_Shop.Application.Comments;
using Tea_Shop.Domain.Comments;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public class CommentsEfCoreRepository : ICommentsRepository
{
    private readonly ProductsDbContext _dbContext;

    public CommentsEfCoreRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Guid> GetTag(Guid tagId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> CreateComment(Comment comment, CancellationToken cancellationToken)
    {
        await _dbContext.Comments.AddAsync(comment, cancellationToken);

        return comment.Id.Value;
    }

    public async Task<Guid> DeleteTag(Guid tagId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}