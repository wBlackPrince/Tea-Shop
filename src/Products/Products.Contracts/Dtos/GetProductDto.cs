namespace Products.Contracts.Dtos;

public record GetProductDto(
    Guid Id,
    string Title,
    float Price,
    float Amount,
    int StockQuantity,
    int SumRatings,
    int CountRatings,
    string Description,
    string Season,
    GetIngrendientsResponseDto[] Ingredients,
    string PreparationDescription,
    int PreparationTime,
    DateTime CreateAt,
    DateTime UpdatedAt,
    Guid[] TagsIds,
    Guid[] PhotosIds);