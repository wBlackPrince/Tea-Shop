namespace Tea_Shop.Contract.Products;

public record CreateIngrindientRequestDto(
    string Name,
    float Amount,
    string Description,
    bool IsAllergen);