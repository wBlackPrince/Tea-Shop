namespace Tea_Shop.Contract.Products;

public record CreateProductRequestDto(
    string Title,
    float Price,
    float Amount,
    int StockQuantity,
    string Description,
    string Season,
    CreateIngrindientRequestDto[] Ingridients,
    string PreparationDescription,
    int PreparationTime,
    Guid[] TagsIds,
    Guid[] PhotosIds);