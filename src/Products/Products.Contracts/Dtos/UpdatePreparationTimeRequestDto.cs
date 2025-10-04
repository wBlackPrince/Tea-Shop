namespace Products.Contracts.Dtos;

public record UpdatePreparationTimeRequestDto(Guid ProductId, int PreparationTime);