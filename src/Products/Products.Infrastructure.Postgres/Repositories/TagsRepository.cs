using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Products.Application;
using Products.Domain;
using Shared;
using Shared.ValueObjects;

namespace Products.Infrastructure.Postgres.Repositories;

public class TagsRepository: ITagsRepository
{
    private readonly ProductsDbContext _dbContext;

    public TagsRepository(ProductsDbContext dbContext)
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

    public async Task<Result<Guid, Error>> DeleteTag(TagId tagId, CancellationToken cancellationToken)
    {
        var tag = await _dbContext.Tags
            .Where(t => t.Id == tagId)
            .ExecuteDeleteAsync(cancellationToken);

        return tagId.Value;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}