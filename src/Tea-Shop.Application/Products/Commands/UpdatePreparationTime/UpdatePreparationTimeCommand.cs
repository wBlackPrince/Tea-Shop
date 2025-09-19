using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products.Commands.UpdatePreparationTime;

public record UpdatePreparationTimeCommand(UpdatePreparationTimeRequestDto Request): ICommand;