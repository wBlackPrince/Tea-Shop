using Comments.Contracts;
using Comments.Contracts.Dtos;
using Shared.Abstractions;

namespace Comments.Application.Commands.CreateCommentCommand;

public record CreateCommentCommand(CreateCommentRequestDto Request): ICommand;