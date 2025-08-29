using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Users;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Tea_Shop.Domain.Tags;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Products;

public class ProductsService : IProductsService
{
    private readonly IProductsRepository _productsRepository;
    private readonly ILogger<ProductsService> _logger;
    private readonly IValidator<CreateProductRequestDto> _createProductValidator;
    private readonly IValidator<UpdateProductPriceRequestDto> _updateProductPriceValidator;
    private readonly IValidator<CreateOrderRequestDto> _createOrderValidator;

    public ProductsService(
        IProductsRepository productsRepository,
        ILogger<ProductsService> logger,
        IValidator<CreateProductRequestDto> createProductValidator,
        IValidator<UpdateProductPriceRequestDto> updateProductPriceValidator,
        IValidator<CreateOrderRequestDto> createOrderValidator)
    {
        _productsRepository = productsRepository;
        _logger = logger;
        _createProductValidator = createProductValidator;
        _updateProductPriceValidator = updateProductPriceValidator;
        _createOrderValidator = createOrderValidator;
    }

    public async Task<Result<GetProductResponseDto, Error>> GetProductById(
        Guid productId,
        CancellationToken cancellationToken)
    {
        var (_, isFailure, product, error) = await _productsRepository.GetProductById(
            new ProductId(productId),
            cancellationToken);

        if (isFailure)
        {
            return error;
        }

        var ingrindientsGetDto = product.PreparationMethod.Ingredients
            .Select(i => new GetIngrendientsResponseDto(
                i.Name,
                i.Amount,
                i.Description,
                i.IsAllergen)).ToArray();

        var tagsIds = product.TagsIds
            .Select(i => i.Id.Value)
            .ToArray();

        var productGetDto = new GetProductResponseDto(
            product.Title,
            product.Price,
            product.Amount,
            product.Description,
            product.Season.ToString(),
            ingrindientsGetDto,
            product.PreparationMethod.Description,
            product.PreparationMethod.PreparationTime,
            tagsIds,
            product.PhotosIds);

        _logger.LogInformation("Get product {productId}", productId);

        return productGetDto;
    }


    public async Task<Result<GetProductResponseDto[], Error>> GetProductsByTag(
        Guid tagId,
        CancellationToken cancellationToken)
    {
        var (_, isFailure, products, error) = await _productsRepository.GetProductsByTag(
            new TagId(tagId),
            cancellationToken);

        if (isFailure)
        {
            return error;
        }

        GetProductResponseDto[] response = new GetProductResponseDto[products.Length];

        for (int i = 0; i < products.Length; i++)
        {
            response[i] = new GetProductResponseDto(
                products[i].Title,
                products[i].Price,
                products[i].Amount,
                products[i].Description,
                products[i].Season.ToString(),
                products[i].PreparationMethod.Ingredients
                    .Select(ing =>
                        new GetIngrendientsResponseDto(ing.Name, ing.Amount, ing.Description, ing.IsAllergen))
                    .ToArray(),
                products[i].PreparationMethod.Description,
                products[i].PreparationMethod.PreparationTime,
                products[i].TagsIds.Select(ti => ti.TagId.Value).ToArray(),
                products[i].PhotosIds);
        }

        return response;
    }


    public async Task<Result<GetIngrendientsResponseDto[], Error>> GetProductIngredients(
        Guid productId,
        CancellationToken cancellationToken)
    {
        var ingredientsResult = await _productsRepository.GetProductIngredients(
            new ProductId(productId),
            cancellationToken);

        if (ingredientsResult.IsFailure)
        {
            return ingredientsResult.Error;
        }

        var ingredientsResponse = ingredientsResult.Value
            .Select(i => new GetIngrendientsResponseDto(
                i.Name,
                i.Amount,
                i.Description,
                i.IsAllergen))
            .ToArray();

        return ingredientsResponse;
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
                ingrRequest.Description,
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


    public async Task<Guid> CreateOrder(
        CreateOrderRequestDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _createOrderValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var orderItems = request.Items.Select(i =>
                OrderItem.Create(
                    new OrderItemId(Guid.NewGuid()),
                    new ProductId(i.ProductId),
                    i.Quantity).Value)
            .ToList();

        var order = new Order(
            new OrderId(Guid.NewGuid()),
            new UserId(request.UserId),
            request.DeliveryAddress,
            (PaymentWay)Enum.Parse(typeof(PaymentWay), request.PaymentMethod),
            request.ExpectedTimeDelivery,
            (OrderStatus)Enum.Parse(typeof(OrderStatus), request.Status),
            orderItems,
            DateTime.UtcNow,
            DateTime.UtcNow);

        await _productsRepository.CreateOrder(order, cancellationToken);

        await _productsRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Create order {orderId}", order.Id);

        return order.Id.Value;
    }


    public async Task<Result<Guid, Error>> UpdateProduct(
        Guid productId,
        JsonPatchDocument<Product> productUpdates,
        CancellationToken cancellationToken)
    {
        var (_, isFailure, product, error) = await _productsRepository.GetProductById(
            new ProductId(productId),
            cancellationToken);

        if (isFailure)
        {
            return error;
        }

        productUpdates.ApplyTo(product);
        await _productsRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Update product {productId}", productId);

        return product.Id.Value;
    }

    public async Task<Result<Guid, Error>> DeleteProduct(
        Guid productId,
        CancellationToken cancellationToken)
    {
        var deleteResult = await _productsRepository.DeleteProduct(
            new ProductId(productId),
            cancellationToken);

        if (deleteResult.IsFailure)
        {
            _logger.LogInformation("Failed to delete product {productId}", productId);

            return deleteResult.Error;
        }

        _logger.LogError("Delete product {productId}", productId);

        return productId;
    }
}