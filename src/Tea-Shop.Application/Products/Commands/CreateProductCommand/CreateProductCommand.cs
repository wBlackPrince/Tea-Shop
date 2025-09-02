using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products.Commands.CreateProductCommand;

public record CreateProductCommand(CreateProductRequestDto Request): ICommand;