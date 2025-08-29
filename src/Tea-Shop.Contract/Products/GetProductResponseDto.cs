namespace Tea_Shop.Contract.Products;

public record GetProductResponseDto(
    Guid Id,
    string Title,
    float Price,
    float Amount,
    string Description,
    string Season,
    GetIngrendientsResponseDto[] Ingredients,
    string PreparationDescription,
    int PreparationTime,
    DateTime CreateAt,
    DateTime UpdatedAt,
    Guid[] TagsIds,
    Guid[] PhotosIds);