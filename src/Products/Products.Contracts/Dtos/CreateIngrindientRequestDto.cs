namespace Products.Contracts.Dtos;

public record CreateIngrindientRequestDto(
    string Name,
    float Amount,
    string Description,
    bool IsAllergen);