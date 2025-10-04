using System.Data;
using Comments.Contracts.Dtos;
using Dapper;
using Microsoft.Extensions.Logging;
using Shared.Abstractions;
using Shared.Database;
using Users.Contracts;
using Users.Contracts.Dtos;

namespace Users.Application.Queries.GetUserCommentsQuery;

public class GetUserCommentsHandler(
    IDbConnectionFactory dbConnectionFactory,
    ILogger<GetUserCommentsHandler> logger):
    IQueryHandler<GetUserCommentsResponseDto?, GetUserCommentsQuery>
{
    public async Task<GetUserCommentsResponseDto?> Handle(
        GetUserCommentsQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(GetUserCommentsHandler));

        var connection = await dbConnectionFactory.CreateConnectionAsync(cancellationToken);

        GetUserCommentsResponseDto? commentsDto = null;

        long? totalCount = null;

        List<string> conditions = [];
        var parameters = new DynamicParameters();

        parameters.Add("user_id", query.Request.UserId, DbType.Guid);
        conditions.Add("c.user_id = @user_id");

        if (query.Request.DateFrom is not null)
        {
            parameters.Add("date_from", query.Request.DateFrom?.ToUniversalTime(), DbType.DateTime);
            conditions.Add("c.created_at >= @date_from");
        }

        if (query.Request.DateTo is not null)
        {
            parameters.Add("date_to", query.Request.DateTo?.ToUniversalTime(), DbType.DateTime);
            conditions.Add("c.updated_at <= @date_to");
        }

        parameters.Add("commentsLimit", query.Request.Pagination.PageSize, DbType.Int32);
        parameters.Add(
            "commentsOffset",
            (query.Request.Pagination.Page - 1) * query.Request.Pagination.PageSize,
            DbType.Int32);

        string whereClause = (conditions.Count > 0)
                ? "WHERE " + string.Join(" AND ", conditions)
                : string.Empty;

        await connection.QueryAsync<
            GetUserCommentsResponseDto,
            CommentDto,
            GetUserCommentsResponseDto>(
            $"""
            SELECT
                u.id AS user_id,
                count(*) over () as total_count,
                c.id AS id,
                c.user_id AS user_id,
                c.text AS text,
                c.rating AS rating,
                c.review_id AS review_id,
                c.parent_id AS parent_id,
                c.created_at AS created_at,
                c.updated_at as updated_at
            FROM users AS u INNER JOIN comments AS c ON u.id = c.user_id
            {whereClause}
            LIMIT @commentsLimit
            OFFSET @commentsOffset
            """,
            param: parameters,
            splitOn: "id",
            map: (u, c) =>
            {
                commentsDto ??= u;

                commentsDto.Comments.Add(c);

                return commentsDto;
            });

        if (commentsDto is null)
        {
            logger.LogWarning("User's comments with id {userId}", query.Request.UserId);
        }

        logger.LogDebug("Get user's comments with id {userId}", query.Request.UserId);

        return commentsDto;
    }
}