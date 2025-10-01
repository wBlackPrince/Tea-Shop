namespace Products.Contracts;

public record GetProductDto(
    Guid Id,
    string Title,
    float Price,
    float Amount,
    int StockQuantity,
    string Description,
    string Season,
    GetIngrendientsResponseDto[] Ingredients,
    string PreparationDescription,
    int PreparationTime,
    DateTime CreateAt,
    DateTime UpdatedAt,
    Guid[] TagsIds,
    Guid[] PhotosIds);