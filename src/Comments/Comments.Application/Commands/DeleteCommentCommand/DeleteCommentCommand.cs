using Comments.Contracts;
using Comments.Contracts.Dtos;
using Shared.Abstractions;

namespace Comments.Application.Commands.DeleteCommentCommand;

public record DeleteCommentCommand(CommentWithOnlyIdDto Request): ICommand;