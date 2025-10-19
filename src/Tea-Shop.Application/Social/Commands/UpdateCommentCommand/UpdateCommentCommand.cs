using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract;

namespace Tea_Shop.Application.Social.Commands.UpdateCommentCommand;

public record UpdateCommentCommand(Guid CommentId, UpdateEntityRequestDto Request): ICommand;