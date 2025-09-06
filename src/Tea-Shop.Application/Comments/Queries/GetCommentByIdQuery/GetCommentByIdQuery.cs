using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Comments;

namespace Tea_Shop.Application.Comments.Queries.GetCommentByIdQuery;

public record GetCommentByIdQuery(CommentWithOnlyIdDto WithOnlyId): IQuery;