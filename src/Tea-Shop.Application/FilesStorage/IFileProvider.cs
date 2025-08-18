namespace Tea_Shop.Application.FilesStorage;

public interface IFileProvider
{
    public Task<string> UploadAsync(Stream stream, string key, string bucket);
}