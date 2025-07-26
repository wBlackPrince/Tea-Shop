using TeaShopDomain.Tags;

namespace TeaShop.Application.Tags;

public interface ITagsRepository
{
    Task<Guid> AddAsync(Tag tag, CancellationToken cancellationToken);

    Task<Guid> SaveAsync(Tag comment, CancellationToken cancellationToken);

    Task<Guid> DeleteAsync(Tag commentId, CancellationToken cancellationToken);
}