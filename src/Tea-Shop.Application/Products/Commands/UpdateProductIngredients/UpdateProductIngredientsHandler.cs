using System.Data;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Commands.UpdateProductIngredients;

public class UpdateProductIngredientsHandler:
    ICommandHandler<ProductWithOnlyIdDto, UpdateProductIngredientsCommand>
{
    private readonly IProductsRepository _productsRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly ILogger<UpdateProductIngredientsHandler> _logger;
    private readonly IValidator<UpdateProductIngredientsCommand> _validator;

    public UpdateProductIngredientsHandler(
        IProductsRepository productsRepository,
        ILogger<UpdateProductIngredientsHandler> logger,
        ITransactionManager transactionManager,
        IValidator<UpdateProductIngredientsCommand> validator)
    {
        _productsRepository = productsRepository;
        _transactionManager = transactionManager;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<ProductWithOnlyIdDto, Error>> Handle(
        UpdateProductIngredientsCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handleName}", nameof(UpdateProductIngredientsHandler));

        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction");
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Failed to update product's ingredients with id {productId}", request.ProductId);
            transactionScope.Rollback();
            return Error.Validation(
                "update.ingridients",
                "Validation failed",
                validationResult.Errors.First().ErrorMessage);
        }

        var product = await _productsRepository.GetProductById(
            request.ProductId,
            cancellationToken);

        if (product is null)
        {
            _logger.LogError("Product with id {productId} not found", request.ProductId);
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

        await _transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Fail to commit transaction");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        _logger.LogDebug("Updated product's ingrendients with id {productId}", request.ProductId);


        return new ProductWithOnlyIdDto(request.ProductId);
    }
}