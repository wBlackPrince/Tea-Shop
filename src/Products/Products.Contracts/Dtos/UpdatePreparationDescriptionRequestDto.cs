namespace Products.Contracts.Dtos;

public record UpdatePreparationDescriptionRequestDto(Guid ProductId, string Description);