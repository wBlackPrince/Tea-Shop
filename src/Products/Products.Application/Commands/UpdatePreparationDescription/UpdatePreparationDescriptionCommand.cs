using Products.Contracts.Dtos;
using Shared.Abstractions;

namespace Products.Application.Commands.UpdatePreparationDescription;

public record UpdatePreparationDescriptionCommand(UpdatePreparationDescriptionRequestDto Request): ICommand;