using CSharpFunctionalExtensions;
using Tea_Shop.Contract.Tags;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Tags;

public interface ITagsService
{
    Task<Guid> CreateTag(
        CreateTagRequestDto request,
        CancellationToken cancellationToken);

    Task<Result<Guid, Error>> Delete(
        Guid tagId,
        CancellationToken cancellationToken);
}