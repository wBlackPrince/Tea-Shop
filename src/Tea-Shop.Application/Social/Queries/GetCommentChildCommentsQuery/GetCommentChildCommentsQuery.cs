using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Social;

namespace Tea_Shop.Application.Social.Queries.GetCommentChildCommentsQuery;

public record GetCommentChildCommentsQuery(CommentWithOnlyIdDto WithOnlyId): IQuery;