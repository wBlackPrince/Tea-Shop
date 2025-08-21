using Tea_Shop.Domain.Comments;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public interface ICommentsRepository
{
    Task<Guid> GetTag(Guid tagId, CancellationToken cancellationToken);
    Task<Guid> CreateComment(Comment comment, CancellationToken cancellationToken);
    Task<Guid> DeleteTag(Guid tagId, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}