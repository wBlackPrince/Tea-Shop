using Comments.Contracts;
using Comments.Contracts.Dtos;
using Shared.Abstractions;

namespace Comments.Application.Queries.GetCommentChildCommentsQuery;

public record GetCommentChildCommentsQuery(CommentWithOnlyIdDto WithOnlyId): IQuery;