namespace Tea_Shop.Contract.Products;

public record CreateProductRequestDto(
    string Title,
    float Price,
    float Amount,
    string Description,
    string Season,
    CreateIngrindientRequestDto[] Ingridients,
    string PreparationmMethod,
    Guid[] TagsIds,
    Guid[] PhotosIds);