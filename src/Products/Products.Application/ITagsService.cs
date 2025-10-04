using CSharpFunctionalExtensions;
using Products.Contracts.Dtos;
using Shared;

namespace Products.Application;

public interface ITagsService
{
    Task<Guid> CreateTag(
        CreateTagRequestDto request,
        CancellationToken cancellationToken);

    Task<Result<Guid, Error>> Delete(
        Guid tagId,
        CancellationToken cancellationToken);
}