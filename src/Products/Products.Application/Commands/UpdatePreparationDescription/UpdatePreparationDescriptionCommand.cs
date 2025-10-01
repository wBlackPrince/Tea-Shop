namespace Products.Application.Commands.UpdatePreparationDescription;

public record UpdatePreparationDescriptionCommand(
    UpdatePreparationDescriptionRequestDto Request): ICommand;