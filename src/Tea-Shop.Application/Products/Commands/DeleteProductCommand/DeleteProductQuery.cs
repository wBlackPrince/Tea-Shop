using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.Application.Products.Commands.DeleteProductCommand;

public record DeleteProductQuery(DeleteProductDto Request): ICommand;