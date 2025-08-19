using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;

namespace Tea_Shop.Application.Products;

public class ProductsService : IProductsService
{
    private readonly IProductsRepository _productsRepository;
    private readonly ILogger<ProductsService> _logger;
    private readonly IValidator<CreateProductRequestDto> _createProductValidator;
    private readonly IValidator<UpdateProductPriceRequestDto> _updateProductPriceValidator;

    public ProductsService(
        IProductsRepository productsRepository,
        ILogger<ProductsService> logger,
        IValidator<CreateProductRequestDto> createProductValidator,
        IValidator<UpdateProductPriceRequestDto> updateProductPriceValidator)
    {
        _productsRepository = productsRepository;
        _logger = logger;
        _createProductValidator = createProductValidator;
        _updateProductPriceValidator = updateProductPriceValidator;
    }

    public async Task<Guid> GetProduct(
        Guid productId,
        CancellationToken cancellationToken)
    {
        var product = await _productsRepository.GetProduct(productId, cancellationToken);

        _logger.LogInformation("Get product {productId}", productId);

        return productId;
    }

    public async Task<Guid> CreateProduct(
        CreateProductRequestDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _createProductValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        ProductId productId = new ProductId(Guid.NewGuid());

        Ingrendient[] ingrindients = request.Ingridients
            .Select(ingrRequest => new Ingrendient(
                ingrRequest.Amount,
                ingrRequest.Name,
                ingrRequest.IsAllergen)).ToArray();

        Product product = new Product(
            productId,
            request.Title,
            request.Description,
            request.Price,
            request.Amount,
            (Season)Enum.Parse(typeof(Season), request.Season),
            ingrindients,
            request.TagsIds,
            request.PreparationDescription,
            request.PreparationTime,
            request.PhotosIds);

        await _productsRepository.CreateProduct(product, cancellationToken);

        await _productsRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Create product {productId}", productId);

        return productId.Value;
    }

    public async Task<Guid> UpdateProductPrice(
        Guid productId,
        UpdateProductPriceRequestDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _updateProductPriceValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        await _productsRepository.UpdateProductPrice(productId, request.Price, cancellationToken);

        // Сохранение изменений сущности Product в базе данных

        _logger.LogInformation("Update product price {productId}", productId);

        return productId;
    }

    public async Task<Guid> DeleteProduct(
        Guid productId,
        CancellationToken cancellationToken)
    {
        // проверка валидности

        await _productsRepository.DeleteProduct(productId, cancellationToken);

        // Логгирование об успешном или не успешном изменении

        return productId;
    }
}