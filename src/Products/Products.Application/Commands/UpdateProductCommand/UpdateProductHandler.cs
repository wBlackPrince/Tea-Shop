using System.ComponentModel.DataAnnotations;
using System.Data;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Products.Application.Commands.CreateProductCommand;
using Products.Domain;
using Shared;
using Shared.Database;
using Shared.Dto;

namespace Products.Application.Commands.UpdateProductCommand;

public class UpdateProductHandler(
    IProductsRepository productsRepository,
    ILogger<CreateProductHandler> logger,
    ITransactionManager transactionManager)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid productId,
        UpdateEntityRequestDto request,
        CancellationToken cancellationToken)
    {
        var transactionScopeResult = await transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            logger.LogError("Failed to begin transaction while updating product");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;


        Product? product = await productsRepository.GetProductById(
            productId,
            cancellationToken);

        if (product is null)
        {
            transactionScope.Rollback();
            return Error.NotFound("update product", "product not found");
        }

        try
        {
            switch (request.Property)
            {
                case nameof(product.Title):
                    product.Title = (string)request.NewValue;
                    break;
                case nameof(product.Amount):
                    product.Amount = (float)request.NewValue;
                    break;
                case nameof(product.Description):
                    product.Description = (string)request.NewValue;
                    break;
                case nameof(product.Price):
                    product.Price = (float)request.NewValue;
                    break;
                case nameof(product.StockQuantity):
                    product.StockQuantity = (int)request.NewValue;
                    break;
                case nameof(product.Season):
                    product.Season = (Season)Enum.Parse(typeof(Season), (string)request.NewValue);
                    break;
                default:
                    throw new ValidationException("Invalid property");
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            transactionScope.Rollback();
            return Error.Validation("update.product", e.Message);
        }

        product.UpdatedAt = DateTime.UtcNow.ToUniversalTime();



        await transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            logger.LogError("Failed to commit result while updating product");
            transactionScope.Rollback();
            return commitedResult.Error;
        }



        logger.LogInformation("Update product {productId}", productId);

        return product.Id.Value;
    }
}