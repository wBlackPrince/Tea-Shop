using Dapper;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Comments;
using Tea_Shop.Contract.Users;

namespace Tea_Shop.Application.Users.Queries.GetUserCommentsQuery;

public class GetUserCommentsHandler:
    IQueryHandler<GetUserCommentsResponseDto?, GetUserCommentsQuery>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<GetUserCommentsHandler> _logger;

    public GetUserCommentsHandler(
        IDbConnectionFactory dbConnectionFactory,
        ILogger<GetUserCommentsHandler> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<GetUserCommentsResponseDto?> Handle(
        GetUserCommentsQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {handler}", nameof(GetUserCommentsHandler));

        var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);

        GetUserCommentsResponseDto? commentsDto = null;

        await connection.QueryAsync<
            GetUserCommentsResponseDto,
            CommentDto,
            GetUserCommentsResponseDto>(
            """
            SELECT
                u.id AS user_id,
                c.id AS id,
                c.user_id AS user_id,
                c.text AS text,
                c.rating AS rating,
                c.review_id AS review_id,
                c.parent_id AS parent_id,
                c.created_at AS created_at,
                c.updated_at as updated_at
            FROM users AS u INNER JOIN comments AS c ON u.id = c.user_id
            WHERE u.id = @userId
            LIMIT @commentsLimit
            OFFSET @commentsOffset
            """,
            param: new
            {
                userId = query.Request.UserDto.UserId,
                commentsLimit = query.Request.Pagination.PageSize,
                commentsOffset = (query.Request.Pagination.Page - 1) * query.Request.Pagination.PageSize,
            },
            splitOn: "id",
            map: (u, c) =>
            {
                commentsDto ??= u;

                commentsDto.Comments.Add(c);

                return commentsDto;
            });

        if (commentsDto is null)
        {
            _logger.LogWarning("User's comments with id {userId}", query.Request.UserDto.UserId);
        }

        _logger.LogDebug("Get user's comments with id {userId}", query.Request.UserDto.UserId);

        return commentsDto;
    }
}