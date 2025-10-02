using Products.Contracts.Dtos;
using Shared.Abstractions;

namespace Products.Application.Commands.UpdateProductIngredients;

public record UpdateProductIngredientsCommand(Guid ProductId, IngridientsDto IngridientsDto): ICommand;