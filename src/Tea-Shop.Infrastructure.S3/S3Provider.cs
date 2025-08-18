using Tea_Shop.Application.FilesStorage;

namespace Tea_Shop.Infrastructure.S3;

public class S3Provider: IFileProvider
{
    public Task<string> UploadAsync(Stream stream, string key, string bucket)
    {
        throw new NotImplementedException();
    }
}