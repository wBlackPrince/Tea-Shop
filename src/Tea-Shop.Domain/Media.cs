using CSharpFunctionalExtensions;
using Tea_Shop.Shared;

namespace Tea_Shop.Domain;

public record Media
{
    public string BucketName { get; }

    public string FileName { get; }

    private Media(string bucketName, string fileName)
    {
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

        return new Media(bucketName, fileName);
    }
}