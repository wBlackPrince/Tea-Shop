using System.Data;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Commands.CreateProductCommand;

public class CreateProductHandler: ICommandHandler<CreateProductResponseDto, CreateProductCommand>
{
    private readonly IProductsRepository _productsRepository;
    private readonly ILogger<CreateProductHandler> _logger;
    private readonly IValidator<CreateProductRequestDto> _validator;
    private readonly ITransactionManager _transactionManager;

    public CreateProductHandler(
        IProductsRepository productsRepository,
        ILogger<CreateProductHandler> logger,
        IValidator<CreateProductRequestDto> validator,
        ITransactionManager transactionManager)
    {
        _productsRepository = productsRepository;
        _validator = validator;
        _logger = logger;
        _transactionManager = transactionManager;
    }

    public async Task<Result<CreateProductResponseDto, Error>> Handle(
        CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handler}", nameof(CreateProductHandler));

        var validationResult = await _validator.ValidateAsync(command.Request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Failed validation while creating product");
            return Error.Validation(
                "product.create",
                "validation errors",
                validationResult.Errors.First().PropertyName);
        }


        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(
            IsolationLevel.RepeatableRead,
            cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            _logger.LogError("Failed to begin transaction while creating product");
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
            command.Request.PhotosIds);


        var createResult = await _productsRepository.CreateProduct(product, cancellationToken);

        if (createResult.IsFailure)
        {
            _logger.LogError("Failed to create product");
            transactionScope.Rollback();
            return createResult.Error;
        }


        await _transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();

        if (commitedResult.IsFailure)
        {
            _logger.LogError("Failed to commit result while creating product");
            transactionScope.Rollback();
            return commitedResult.Error;
        }

        _logger.LogDebug("Created product with id {productId}", productId);

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