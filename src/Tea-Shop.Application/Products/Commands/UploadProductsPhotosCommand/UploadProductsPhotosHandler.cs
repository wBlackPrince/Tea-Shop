using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.FilesStorage;
using Tea_Shop.Application.Products.Commands.CreateProductCommand;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Commands.UploadProductsPhotosCommand;

public class UploadProductsPhotosHandler(
    ILogger<CreateProductHandler> logger,
    IProductsRepository productsRepository,
    IValidator<CreateProductRequestDto> validator,
    IFileProvider fileProvider):
    ICommandHandler<Guid, UploadProductsPhotosCommand>
{
    private const string _avatarBucket = "media";

    public async Task<Result<Guid, Error>> Handle(
        UploadProductsPhotosCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(UploadProductsPhotosHandler));

        var product = await productsRepository.GetProductById(command.Request.ProductId, cancellationToken);

        if (product is null)
        {
            logger.LogDebug("Product with id {productId} does not exist", command.Request.ProductId);
            return Error.Failure(
                "upload.product_photos",
                $"product with id {command.Request.ProductId} not found");
        }

        Guid[] productPhotos = new Guid[command.Request.FileDtos.Count()];

        for (int i = 0; i < command.Request.FileDtos.Count(); i++)
        {
            productPhotos[i] = Guid.NewGuid();
        }

        string[] extensions = new string[command.Request.FileDtos.Count()];
        string[] keys = new string[command.Request.FileDtos.Count()];
        Stream[] streams = new Stream[command.Request.FileDtos.Count()];
        var photosNames = command.Request.FileDtos.Select(f => f.FileName).ToArray();

        if (command.Request.FileDtos.Any())
        {
            // расширения файлов
            for (int i = 0; i < command.Request.FileDtos.Count(); i++)
            {
                extensions[i] = Path.GetExtension(command.Request.FileDtos[i].FileName);
            }

            // пути + имена внутри бакета
            for (int i = 0; i < command.Request.FileDtos.Count(); i++)
            {
                keys[i] = $"products/{command.Request.ProductId}/photos/{productPhotos[i]:N}{extensions[i]}";
            }

            for (int i = 0; i < command.Request.FileDtos.Count(); i++)
            {
                // открываем поток
                //await using var stream = command.Request.FileDtos[i].Stream;
                //streams[i] = stream;
                var upload = await fileProvider
                    .UploadAsync(
                        stream: command.Request.FileDtos[i].Stream,
                        key: keys[i],
                        bucket: _avatarBucket,
                        fileName: photosNames[i],
                        createBucketIfNotExists: true,
                        cancellationToken: cancellationToken);

                if (upload.IsFailure)
                {
                    logger.LogError($"avatar upload failed: {upload.Error.Message}");
                    return Error.Failure("create.product", $"photo upload failed: {upload.Error.Message}");
                }
            }

            // var uploads = await _fileProvider
            //     .UploadFilesAsync(
            //         streams: streams,
            //         keys: keys,
            //         bucket: _avatarBucket,
            //         filesNames: photosNames,
            //         createBucketIfNotExists: true,
            //         cancellationToken: cancellationToken);

            // if (uploads.IsFailure)
            // {
            //     _logger.LogError($"avatar upload failed: {uploads.Error.Message}");
            //     return Error.Failure("create.product", $"photos upload failed: {uploads.Error.Message}");
            // }
        }

        logger.LogDebug("Created product with id {productId}", command.Request.ProductId);

        return command.Request.ProductId;
    }
}