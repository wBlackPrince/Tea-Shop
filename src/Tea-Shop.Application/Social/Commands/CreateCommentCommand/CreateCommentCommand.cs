using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Social;

namespace Tea_Shop.Application.Social.Commands.CreateCommentCommand;

public record CreateCommentCommand(CreateCommentRequestDto Request): ICommand;