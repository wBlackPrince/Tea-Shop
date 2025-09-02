using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Comments;

namespace Tea_Shop.Application.Comments.Commands.CreateCommentCommand;

public record CreateCommentCommand(CreateCommentRequestDto Request): ICommand;