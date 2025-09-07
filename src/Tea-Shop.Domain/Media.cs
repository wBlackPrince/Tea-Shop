using CSharpFunctionalExtensions;
using Tea_Shop.Shared;

namespace Tea_Shop.Domain;

public record Media
{
    public Guid Id { get; }

    public string BucketName { get; }

    public string FileName { get; }

    public string Key => FileName;

    private Media(Guid id, string bucketName, string fileName)
    {
        Id = id;
        BucketName = bucketName;
        FileName = fileName;
    }

    public static Result<Media, Error> Create(string bucketName, string fileName)
    {
        if (string.IsNullOrWhiteSpace(bucketName) || string.IsNullOrWhiteSpace(fileName))
        {
            return Error.Validation(
                "create.media",
                "Backet's name or filename must be not empty");
        }

        return new Media(Guid.NewGuid(), bucketName, fileName);
    }
}