using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products.Commands.UpdateProductIngredients;

public record UpdateProductIngredientsCommand(Guid ProductId, IngridientsDto IngridientsDto): ICommand;