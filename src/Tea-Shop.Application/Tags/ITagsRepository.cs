using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Tags;

namespace Tea_Shop.Application.Tags;

public interface ITagsRepository
{
    Task<Guid> GetTag(Guid tagId, CancellationToken cancellationToken);

    Task<Guid> CreateTag(Tag tag, CancellationToken cancellationToken);

    Task<Guid> DeleteTag(Guid tagId, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}