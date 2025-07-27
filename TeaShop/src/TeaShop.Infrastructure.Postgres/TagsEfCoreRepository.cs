using TeaShop.Application.Tags;
using TeaShopDomain.Tags;

namespace TeaShop.Infrastructure.Postgres;

public class TagsEfCoreRepository: ITagsRepository
{
    public async Task<Guid> AddAsync(Tag tag, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> SaveAsync(Tag comment, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> DeleteAsync(Tag commentId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}