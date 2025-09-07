using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Tea_Shop.Domain;
using Tea_Shop.Shared;

namespace Tea_Shop.Infrastructure.S3;

public class MinioProvider: Tea_Shop.Application.FilesStorage.IFileProvider
{
    private readonly int MAX_STREAMS_LENGHT = 5;
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioProvider> _logger;

    public MinioProvider(IMinioClient minioClient, ILogger<MinioProvider> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<Result<Media, Error>> UploadAsync(
        Stream stream,
        string key,
        string bucket,
        string fileName,
        bool createBucketIfNotExists,
        CancellationToken cancellationToken)
    {
        var isBucketExists = await CheckIsExistsBucket(
            bucket,
            cancellationToken);

        if (isBucketExists.IsFailure && !createBucketIfNotExists)
        {
            string failMessage = "Bucket с именем {bucketName} не найден";
            _logger.LogError(failMessage);
            return Error.Failure("upload.file", failMessage);
        }
        else
        {
            var makeBucketArgs = new MakeBucketArgs()
                .WithBucket(bucket);
            await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
        }

        PutObjectArgs minioFileArgs = new PutObjectArgs()
            .WithBucket(bucket.ToLower())
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithObject(key);

        var result = await _minioClient.PutObjectAsync(
            minioFileArgs,
            cancellationToken);

        string message = $"Файл {fileName} загружен в {bucket} бакет";
        _logger.LogInformation(message);

        //TODO раньше возвращал Media.Create(bucket, key)
        return Media.Create(bucket, key);
    }

    private async Task<Result<string, Error>> CheckIsExistsBucket(
        string bucketName,
        CancellationToken cancellationToken)
    {
        // получаем бакеты
        var buckets = await _minioClient.ListBucketsAsync(
            cancellationToken: cancellationToken);
        IReadOnlyList<string> bucketNames = buckets.Buckets.Select(
            b => b.Name.ToLower()).ToList();

        if (!bucketNames.Any(name => name == bucketName))
        {
            _logger.LogError("Bucket с именем {0} не существует", bucketName);
            return Error.NotFound(
                "check.existing.bucket",
                $"Bucket с именем {bucketName}");
        }

        return bucketName;
    }
}