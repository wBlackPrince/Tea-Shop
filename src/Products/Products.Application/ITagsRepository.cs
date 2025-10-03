using CSharpFunctionalExtensions;
using Products.Domain;
using Shared;
using Shared.ValueObjects;

namespace Products.Application;

public interface ITagsRepository
{
    Task<Guid> GetTag(Guid tagId, CancellationToken cancellationToken);

    Task<Guid> CreateTag(Tag tag, CancellationToken cancellationToken);

    Task<Result<Guid, Error>> DeleteTag(TagId tagId, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}