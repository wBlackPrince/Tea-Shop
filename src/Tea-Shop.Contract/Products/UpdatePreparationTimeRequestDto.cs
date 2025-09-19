namespace Tea_Shop.Contract.Products;

public record UpdatePreparationTimeRequestDto(Guid ProductId, int PreparationTime);