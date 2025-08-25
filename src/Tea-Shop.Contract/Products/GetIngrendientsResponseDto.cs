namespace Tea_Shop.Contract.Products;

public record GetIngrendientsResponseDto(
    string Name,
    float Amount,
    string Description,
    bool IsAllergen);