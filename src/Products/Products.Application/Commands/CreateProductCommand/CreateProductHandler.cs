using System.Data;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Products.Contracts.Dtos;
using Products.Domain;
using Shared;
using Shared.Abstractions;
using Shared.Database;
using Shared.ValueObjects;

namespace Products.Application.Commands.CreateProductCommand;

public class CreateProductHandler(
    IProductsRepository productsRepository,
    ILogger<CreateProductHandler> logger,
    IValidator<CreateProductRequestDto> validator,
    ITransactionManager transactionManager): ICommandHandler<CreateProductResponseDto, CreateProductCommand>
{
    public async Task<Result<CreateProductResponseDto, Error>> Handle(
        CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(CreateProductHandler));

        var validationResult = await validator.ValidateAsync(command.Request, cancellationToken);

        if (!validationResult.IsValid)
        {
            logger.LogError("Failed validation while creating product");
            return Error.Validation(
                "product.create",
                "validation errors",
                validationResult.Errors.First().PropertyName);
        }


        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while creating product");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        ProductId productId = new ProductId(Guid.NewGuid());

        Ingrendient[] ingrindients = command.Request.Ingridients
            .Select(ingrRequest => new Ingrendient(
                ingrRequest.Amount,
                ingrRequest.Name,
                ingrRequest.Description,
                ingrRequest.IsAllergen)).ToArray();

        Product product = new Product(
            productId,
            command.Request.Title,
            command.Request.Description,
            command.Request.Price,
            command.Request.Amount,
            command.Request.StockQuantity,
            (Season)Enum.Parse(typeof(Season), command.Request.Season),
            ingrindients,
            command.Request.TagsIds,
            command.Request.PreparationDescription,
            command.Request.PreparationTime,
            []);


        var createResult = await productsRepository.CreateProduct(product, cancellationToken);

        if (createResult.IsFailure)
        {
            logger.LogError("Failed to create product");
            transactionScope.Rollback();
            return createResult.Error;
        }


        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while creating product");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        logger.LogDebug("Created product with id {productId}", productId);

        var productDto = new CreateProductResponseDto(
            product.Id.Value,
            product.Title,
            product.Price,
            product.Amount,
            product.StockQuantity,
            product.Description,
            product.Season.ToString(),
            product.PreparationMethod.Ingredients.Select(ingr =>
                new GetIngrendientsResponseDto(
                    ingr.Name,
                    ingr.Amount,
                    ingr.Description,
                    ingr.IsAllergen)).ToArray(),
            product.PreparationMethod.Description,
            product.PreparationMethod.PreparationTime,
            product.CreatedAt,
            product.UpdatedAt,
            product.TagsIds.Select(t => t.TagId.Value).ToArray(),
            product.PhotosIds);

        return productDto;
    }
}