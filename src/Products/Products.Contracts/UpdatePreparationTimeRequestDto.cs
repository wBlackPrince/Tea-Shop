namespace Products.Contracts;

public record UpdatePreparationTimeRequestDto(Guid ProductId, int PreparationTime);