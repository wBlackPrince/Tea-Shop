using Comments.Contracts;
using Comments.Contracts.Dtos;
using Shared.Abstractions;

namespace Comments.Application.Queries.GetCommentByIdQuery;

public record GetCommentByIdQuery(CommentWithOnlyIdDto WithOnlyId): IQuery;