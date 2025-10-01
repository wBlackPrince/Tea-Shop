namespace Comments.Application.Commands.DeleteCommentCommand;

public record DeleteCommentCommand(CommentWithOnlyIdDto Request): ICommand;