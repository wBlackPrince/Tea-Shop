using TeaShop.Application.FilesStorage;

namespace TeaShop.Infrastructure.S3;

public class S3Prodiver: IFilesProvider
{
    public Task<string> UploadAsync(Stream stream, string key, string bucket)
    {
        throw new NotImplementedException();
    }
}