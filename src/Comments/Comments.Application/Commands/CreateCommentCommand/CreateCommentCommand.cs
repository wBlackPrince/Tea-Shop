namespace Comments.Application.Commands.CreateCommentCommand;

public record CreateCommentCommand(CreateCommentRequestDto Request): ICommand;