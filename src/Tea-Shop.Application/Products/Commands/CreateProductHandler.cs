using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products.Commands;

public class CreateProductHandler
{
    private readonly IProductsRepository _productsRepository;
    private readonly ILogger<CreateProductHandler> _logger;
    private readonly IValidator<CreateProductRequestDto> _validator;

    public CreateProductHandler(
        IProductsRepository productsRepository,
        ILogger<CreateProductHandler> logger,
        IValidator<CreateProductRequestDto> validator)
    {
        _productsRepository = productsRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<CreateProductResponseDto, Error>> Handle(
        CreateProductRequestDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Error.Validation("product.create", "validation errors");
        }

        ProductId productId = new ProductId(Guid.NewGuid());

        Ingrendient[] ingrindients = request.Ingridients
            .Select(ingrRequest => new Ingrendient(
                ingrRequest.Amount,
                ingrRequest.Name,
                ingrRequest.Description,
                ingrRequest.IsAllergen)).ToArray();

        Product product = new Product(
            productId,
            request.Title,
            request.Description,
            request.Price,
            request.Amount,
            request.StockQuantity,
            (Season)Enum.Parse(typeof(Season), request.Season),
            ingrindients,
            request.TagsIds,
            request.PreparationDescription,
            request.PreparationTime,
            request.PhotosIds);

        await _productsRepository.CreateProduct(product, cancellationToken);

        await _productsRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Create product {productId}", productId);

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