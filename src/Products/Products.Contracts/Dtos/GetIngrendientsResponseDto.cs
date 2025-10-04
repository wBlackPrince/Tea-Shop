namespace Products.Contracts.Dtos;

public record GetIngrendientsResponseDto(
    string Name,
    float Amount,
    string Description,
    bool IsAllergen);