using Dapper;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Comments;

namespace Tea_Shop.Application.Comments.Queries.GetCommentChildCommentsQuery;

public class GetCommentChildCommentsHandler:
    IQueryHandler<GetChildCommentsResponseDto, GetCommentChildCommentsQuery>
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<GetCommentChildCommentsHandler> _logger;

    public GetCommentChildCommentsHandler(
        IDbConnectionFactory connectionFactory,
        ILogger<GetCommentChildCommentsHandler> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<GetChildCommentsResponseDto> Handle(
        GetCommentChildCommentsQuery query,
        CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        var childComments = await connection.QueryAsync<CommentDto>(
            """
            select
                c2.id,
                c2.user_id,
                c2.text,
                c2.rating,
                c2.review_id,
                c2.parent_id,
                c2.created_at,
                c2.updated_at
            from comments as c1 inner join comments as c2 on c1.id = c2.parent_id
            where c1.id = @commentId
            """,
            param: new { commentId = query.Request.CommentId });

        var response = new GetChildCommentsResponseDto(
            childComments.ToArray());

        return response;
    }
}