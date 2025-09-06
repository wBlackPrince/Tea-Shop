using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Comments;

namespace Tea_Shop.Application.Comments.Queries.GetCommentChildCommentsQuery;

public record GetCommentChildCommentsQuery(CommentWithOnlyIdDto WithOnlyId): IQuery;