using Products.Contracts.Dtos;
using Shared.Abstractions;

namespace Products.Application.Commands.DeleteProductCommand;

public record DeleteProductQuery(DeleteProductDto Request): ICommand;