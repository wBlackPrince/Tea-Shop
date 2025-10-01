namespace Products.Contracts;

public record GetIngrendientsResponseDto(
    string Name,
    float Amount,
    string Description,
    bool IsAllergen);