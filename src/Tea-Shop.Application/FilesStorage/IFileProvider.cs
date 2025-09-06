using CSharpFunctionalExtensions;
using Tea_Shop.Domain;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.FilesStorage;

public interface IFileProvider
{
    Task<Result<Media, Error>> UploadAsync(
        Stream stream,
        string key,
        string bucket,
        string fileName,
        bool createBucketIfNotExists,
        CancellationToken cancellationToken);
}