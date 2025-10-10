using Dapper;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Social;

namespace Tea_Shop.Application.Social.Queries.GetNeighboursQuery;

public class GetNeighboursHandler(
    ILogger<GetNeighboursHandler> logger,
    IDbConnectionFactory connectionFactory): IQueryHandler<CommentsResponseDto, GetNeighboursQuery>
{
    public async Task<CommentsResponseDto> Handle(
        GetNeighboursQuery query,
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
            from comments as c cross join comment_path as cp
            where
                subpath(cp.path, 0, nlevel(cp.path) - 1) = subpath(c.path, 0, nlevel(c.path) - 1) and
                c.review_id = cp.review_id and cp.path != c.path
            """,
            param: new
            {
                commentId = query.Request.CommentId,
            });

        return new CommentsResponseDto(hierarchy.ToArray());
    }
}