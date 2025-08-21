using Tea_Shop.Contract.Tags;

namespace Tea_Shop.Application.Tags;

public interface ITagsService
{
    Task<Guid> CreateTag(
        CreateTagRequestDto request,
        CancellationToken cancellationToken);
}