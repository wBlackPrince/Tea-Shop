namespace Tea_Shop.Contract.Products;

public record CreateIngrindientRequestDto(
    string name,
    float amount,
    string description,
    bool is_allergen);