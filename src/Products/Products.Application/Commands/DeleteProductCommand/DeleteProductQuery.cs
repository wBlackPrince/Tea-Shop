namespace Products.Application.Commands.DeleteProductCommand;

public record DeleteProductQuery(DeleteProductDto Request): ICommand;