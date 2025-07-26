using FluentValidation;
using Microsoft.Extensions.Logging;
using TeaShop.Contract.Products;
using TeaShopDomain.Products;

namespace TeaShop.Application.Products;

public class ProductService : IProductService
{
    private readonly IProductsRepository _productsRepository;
    private readonly ILogger<ProductService> _logger;
    private readonly IValidator<CreateProductDto> _createProductValidator;

    public ProductService(
        IProductsRepository productsRepository,
        ILogger<ProductService> logger,
        IValidator<CreateProductDto> createProductValidator)
    {
        _productsRepository = productsRepository;
        _logger = logger;
        _createProductValidator = createProductValidator;
    }

    public async Task<Guid> Create(CreateProductDto request, CancellationToken cancellationToken)
    {
        var result = await _createProductValidator.ValidateAsync(request, cancellationToken);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }

        int countOfProductsWithSimilarName = await _productsRepository.GetProductCountWithSimilarName(
            request.Title,
            cancellationToken);

        if (countOfProductsWithSimilarName > 0)
        {
            throw new ValidationException("Продукт с таким названием уже существует");
        }

        Guid id = Guid.NewGuid();
        Product product = new Product(
            id,
            request.Title,
            request.Price,
            request.Amount,
            request.Description,
            request.TagsIds,
            request.PhotosIds
        );

        await _productsRepository.AddAsync(product, cancellationToken);

        // сохранение продукта в БД

        _logger.LogInformation("Product {id} created.", id);

        return id;
    }
}