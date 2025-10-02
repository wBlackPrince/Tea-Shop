using System.Data;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Products.Contracts.Dtos;
using Products.Domain;
using Shared;
using Shared.Abstractions;
using Shared.Database;

namespace Products.Application.Commands.UpdateProductIngredients;

public class UpdateProductIngredientsHandler(
    IProductsRepository productsRepository,
    ILogger<UpdateProductIngredientsHandler> logger,
    ITransactionManager transactionManager,
    IValidator<UpdateProductIngredientsCommand> validator):
    ICommandHandler<ProductWithOnlyIdDto, UpdateProductIngredientsCommand>
{
    public async Task<Result<ProductWithOnlyIdDto, Error>> Handle(
        UpdateProductIngredientsCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handleName}", nameof(UpdateProductIngredientsHandler));

        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            logger.LogError("Failed to update product's ingredients with id {productId}", request.ProductId);
            transactionScope.Rollback();
            return Error.Validation(
                "update.ingridients",
                "Validation failed",
                validationResult.Errors.First().ErrorMessage);
        }

        var product = await productsRepository.GetProductById(
            request.ProductId,
            cancellationToken);

        if (product is null)
        {
            logger.LogError("Product with id {productId} not found", request.ProductId);
            transactionScope.Rollback();
            return Error.NotFound("product.get", "Product not found");
        }

        var newIngrendients = request.IngridientsDto.Ingridients
            .Select(i => new Ingrendient(
                i.Amount,
                i.Name,
                i.Description,
                i.IsAllergen)).ToArray();

        product.UpdateIngredients(newIngrendients);

        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Fail to commit transaction");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        logger.LogDebug("Updated product's ingrendients with id {productId}", request.ProductId);


        return new ProductWithOnlyIdDto(request.ProductId);
    }
}