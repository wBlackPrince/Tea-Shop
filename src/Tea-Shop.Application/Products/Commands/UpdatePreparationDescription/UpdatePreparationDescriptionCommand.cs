using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products.Commands.UpdatePreparationDescription;

public record UpdatePreparationDescriptionCommand(
    UpdatePreparationDescriptionRequestDto Request): ICommand;