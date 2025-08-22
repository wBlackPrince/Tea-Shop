namespace Tea_Shop.Contract.Products;

public record GetProductResponseDto(
    string Title,
    float Price,
    float Amount,
    string Description,
    string Season,
    GetIngrendientsResponseDto[] Ingridients,
    string PreparationDescription,
    int PreparationTime,
    Guid[] TagsIds,
    Guid[] PhotosIds);