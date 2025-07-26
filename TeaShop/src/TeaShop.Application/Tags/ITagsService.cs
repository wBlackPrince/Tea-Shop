using TeaShop.Contract.Tags;

namespace TeaShop.Application.Tags;

public interface ITagsService
{
    Task<Guid> Create(CreateTagDto request, CancellationToken cancellationToken);
}