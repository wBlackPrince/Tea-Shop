namespace Products.Application.Commands.CreateProductCommand;

public record CreateProductCommand(CreateProductRequestDto Request): ICommand;