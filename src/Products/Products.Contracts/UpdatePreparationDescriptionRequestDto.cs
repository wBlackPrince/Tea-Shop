namespace Products.Contracts;

public record UpdatePreparationDescriptionRequestDto(Guid ProductId, string Description);