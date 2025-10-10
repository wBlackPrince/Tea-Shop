using Dapper;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Social;

namespace Tea_Shop.Application.Social.Queries.GetCommentChildCommentsQuery;

public class GetCommentChildCommentsHandler(
    IDbConnectionFactory connectionFactory,
    ILogger<GetCommentChildCommentsHandler> logger):
    IQueryHandler<CommentsResponseDto, GetCommentChildCommentsQuery>
{
    public async Task<CommentsResponseDto> Handle(
        GetCommentChildCommentsQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(GetCommentChildCommentsHandler));

        using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        var childComments = (await connection.QueryAsync<CommentDto>(
            """
            select
                c2.id,
                c2.user_id,
                c2.text,
                c2.rating,
                c2.review_id,
                c2.parent_id,
                c2.path,
                c2.created_at,
                c2.updated_at
            from comments as c1 inner join comments as c2 on c1.id = c2.parent_id
            where c1.id = @commentId
            """,
            param: new { commentId = query.WithOnlyId.CommentId })).ToArray();

        if (childComments.Length == 0)
        {
            logger.LogWarning(
                "Children comments from comment with id {commentId} not found.",
                query.WithOnlyId.CommentId);
        }

        var response = new CommentsResponseDto(
            childComments.ToArray());

        logger.LogDebug(
            "Get children comments from comment with id {commentId}.",
            query.WithOnlyId.CommentId);

        return response;
    }
}