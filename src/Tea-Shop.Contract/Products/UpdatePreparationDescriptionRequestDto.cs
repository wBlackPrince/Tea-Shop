namespace Tea_Shop.Contract.Products;

public record UpdatePreparationDescriptionRequestDto(Guid ProductId, string Description);