using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products.Commands.CreateTagCommand;

public record CreateTagCommand(CreateTagRequestDto Request): ICommand;