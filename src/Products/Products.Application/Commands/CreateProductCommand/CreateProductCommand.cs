using Products.Contracts.Dtos;
using Shared.Abstractions;

namespace Products.Application.Commands.CreateProductCommand;

public record CreateProductCommand(CreateProductRequestDto Request): ICommand;