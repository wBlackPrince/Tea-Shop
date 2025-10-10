using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Social;

namespace Tea_Shop.Application.Social.Queries.GetCommentByIdQuery;

public record GetCommentByIdQuery(CommentWithOnlyIdDto WithOnlyId): IQuery;