using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Comments;

namespace Tea_Shop.Application.Comments.Queries.GetNeighboursQuery;

public record GetNeighboursQuery(CommentWithOnlyIdDto Request): IQuery;