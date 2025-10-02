using Products.Contracts.Dtos;
using Shared.Abstractions;

namespace Products.Application.Commands.UpdatePreparationTime;

public record UpdatePreparationTimeCommand(UpdatePreparationTimeRequestDto Request): ICommand;