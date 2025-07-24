namespace TeaShop.Contract.Products;

public record CreateProductDto(
    string Title,
    int Price,
    int Amount,
    string Description,
    string[] Ingridients,
    string Allergens,
    string PreparationMethod,
    Guid[] TagsIds,
    Guid[] PhotosIds);