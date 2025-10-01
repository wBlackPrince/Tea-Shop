namespace Comments.Application.Queries.GetCommentByIdQuery;

public record GetCommentByIdQuery(CommentWithOnlyIdDto WithOnlyId): IQuery;