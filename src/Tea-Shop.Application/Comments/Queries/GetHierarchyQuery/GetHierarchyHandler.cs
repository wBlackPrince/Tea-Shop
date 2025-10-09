using Dapper;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Comments;

namespace Tea_Shop.Application.Comments.Queries.GetHierarchyQuery;

public class GetHierarchyHandler(
    ILogger<GetHierarchyHandler> logger,
    IDbConnectionFactory connectionFactory): IQueryHandler<CommentsResponseDto, GetHierarchyQuery>
{
    public async Task<CommentsResponseDto> Handle(GetHierarchyQuery query, CancellationToken cancellationToken)
    {
        var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        var hierarchy = await connection.QueryAsync<CommentDto>(
            """
                with comment_path as (
                    select 
                            id as comment_id, 
                            review_id,
                            path 
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
                where c.review_id = cp.review_id
                """,
            param: new
            {
                commentId = query.Request.CommentId,
            });

        return new CommentsResponseDto(hierarchy.ToArray());
    }
}