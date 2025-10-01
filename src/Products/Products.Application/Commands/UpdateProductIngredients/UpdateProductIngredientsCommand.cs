namespace Products.Application.Commands.UpdateProductIngredients;

public record UpdateProductIngredientsCommand(Guid ProductId, IngridientsDto IngridientsDto): ICommand;