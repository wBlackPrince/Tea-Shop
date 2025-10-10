using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract;

namespace Tea_Shop.Application.Products.Commands.UpdateProductCommand;

public record UpdateProductCommand(
    Guid ProductId,
    UpdateEntityRequestDto ProductUpdates): ICommand;