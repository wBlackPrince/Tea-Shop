using CSharpFunctionalExtensions;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Tags;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Tags;

public interface ITagsRepository
{
    Task<Guid> GetTag(Guid tagId, CancellationToken cancellationToken);

    Task<Guid> CreateTag(Tag tag, CancellationToken cancellationToken);

    Task<Result<Guid, Error>> DeleteTag(TagId tagId, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}