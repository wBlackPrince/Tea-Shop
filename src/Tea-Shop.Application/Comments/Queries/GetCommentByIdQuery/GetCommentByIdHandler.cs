using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Comments;
using Tea_Shop.Domain.Comments;

namespace Tea_Shop.Application.Comments.Queries.GetCommentByIdQuery;

public class GetCommentByIdHandler(
    IReadDbContext readDbContext,
    ILogger<GetCommentByIdHandler> logger):
    IQueryHandler<CommentDto?, GetCommentByIdQuery>
{
    public async Task<CommentDto?> Handle(
        GetCommentByIdQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(GetCommentByIdHandler));

        var comment = await readDbContext.CommentsRead.FirstOrDefaultAsync(
            c => c.Id == new CommentId(query.WithOnlyId.CommentId),
            cancellationToken);

        if (comment is null)
        {
            logger.LogWarning("Comment with id {commentId} not found", query.WithOnlyId.CommentId);
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
            comment.UpdatedAt);

        logger.LogDebug("Get comment with id {commentId}.", query.WithOnlyId.CommentId);

        return response;
    }
}