using CSharpFunctionalExtensions;

namespace Shared.FilesStorage;

public interface IFileProvider
{
    Task<Result<Media, Error>> UploadAsync(
        Stream stream,
        string key,
        string bucket,
        string fileName,
        bool createBucketIfNotExists,
        CancellationToken cancellationToken);

    //Task DeleteFile();

    //Task DownloadFile();

    //Task GetFilePresignedPath();
}