using Dapper;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Comments;

namespace Tea_Shop.Application.Comments.Queries.GetDescendantsQuery;

public class GetDescendantsHandler(
    ILogger<GetDescendantsHandler> logger,
    IDbConnectionFactory connectionFactory): IQueryHandler<CommentsResponseDto, GetDescendantsQuery>
{
    public async Task<CommentsResponseDto> Handle(
        GetDescendantsQuery query,
        CancellationToken cancellationToken)
    {
        var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        var hierarchy = await connection.QueryAsync<CommentDto>(
            """
            with comment_path as (
                select 
                        id as comment_id, 
                        path,
                        review_id
                from comments
                where id = @commentId
            )
            
            select
                c.id,
                c.user_id,
                c.text,
                c.rating,
                c.review_id,
                c.parent_id,
                c.path,
                c.created_at,
                c.updated_at
            from comments as c join comment_path as cp on c.path <@ cp.path
            where
                subpath(cp.path, 0, nlevel(cp.path) - 1) = subpath(c.path, 0, nlevel(c.path) - 1) and
                c.review_id = cp.review_id and 
                cp.path != c.path
            """,
            param: new
            {
                commentId = query.Request.CommentId,
                depth = query.Request.Depth,
            });

        return new CommentsResponseDto(hierarchy.ToArray());
    }
}