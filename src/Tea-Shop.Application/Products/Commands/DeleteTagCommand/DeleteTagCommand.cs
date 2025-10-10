using Tea_Shop.Application.Abstractions;

namespace Tea_Shop.Application.Products.Commands.DeleteTagCommand;

public record DeleteTagCommand(Guid TagId): ICommand;