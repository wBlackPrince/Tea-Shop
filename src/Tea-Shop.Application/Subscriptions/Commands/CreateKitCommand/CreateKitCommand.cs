using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Subscriptions;

namespace Tea_Shop.Application.Subscriptions.Commands.CreateKitCommand;

public record CreateKitCommand(CreateKitRequestDto Request): ICommand;