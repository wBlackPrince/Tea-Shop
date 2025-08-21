using Tea_Shop.Application.Tags;
using Tea_Shop.Domain.Tags;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public class TagsEfCoreRepository: ITagsRepository
{
    private readonly ProductsDbContext _dbContext;

    public TagsEfCoreRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Guid> GetTag(Guid tagId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> CreateTag(Tag tag, CancellationToken cancellationToken)
    {
        await _dbContext.Tags.AddAsync(tag, cancellationToken);

        return tag.Id.Value;
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