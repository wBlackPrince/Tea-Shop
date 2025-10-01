namespace Products.Contracts;

public record CreateIngrindientRequestDto(
    string Name,
    float Amount,
    string Description,
    bool IsAllergen);