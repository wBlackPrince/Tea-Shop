using Tea_Shop.Application.Abstractions;
using Tea_Shop.Contract.Comments;

namespace Tea_Shop.Application.Comments.Queries.GetHierarchyQuery;

public record GetHierarchyQuery(CommentWithOnlyIdDto Request): IQuery;