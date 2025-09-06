using Microsoft.EntityFrameworkCore;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Comments;
using Tea_Shop.Domain.Comments;

namespace Tea_Shop.Application.Comments.Queries.GetCommentByIdQuery;

public class GetCommentByIdHandler:
    IQueryHandler<CommentDto?, GetCommentByIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetCommentByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<CommentDto?> Handle(
        GetCommentByIdQuery query,
        CancellationToken cancellationToken)
    {
        var comment = await _readDbContext.CommentsRead.FirstOrDefaultAsync(
            c => c.Id == new CommentId(query.WithOnlyId.CommentId),
            cancellationToken);

        if (comment is null)
        {
            return null;
        }

        var response = new CommentDto(
            comment.Id.Value,
            comment.UserId.Value,
            comment.Text,
            comment.Rating,
            comment.ReviewId.Value,
            comment.ParentId?.Value,
            comment.CreatedAt,
            comment.UpdatedAt
        );

        return response;
    }
}