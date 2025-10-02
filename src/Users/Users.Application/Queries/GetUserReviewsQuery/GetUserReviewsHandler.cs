using Dapper;
using Microsoft.Extensions.Logging;
using Reviews.Contracts.Dtos;
using Shared.Abstractions;
using Shared.Database;
using Users.Contracts;
using Users.Contracts.Dtos;

namespace Users.Application.Queries.GetUserReviewsQuery;

public class GetUserReviewsHandler(
    IDbConnectionFactory dbConnectionFactory,
    ILogger<GetUserReviewsHandler> logger):
    IQueryHandler<GetUserReviewsResponseDto?, GetUserReviewsQuery>
{
    public async Task<GetUserReviewsResponseDto?> Handle(
        GetUserReviewsQuery query,
        CancellationToken cancellationToken)
    {
        var connection = await dbConnectionFactory.CreateConnectionAsync(cancellationToken);

        GetUserReviewsResponseDto? userReviewsDto = null;

        var reviews = await connection.QueryAsync
            <GetUserReviewsResponseDto, ReviewDto, GetUserReviewsResponseDto>(
                """
                SELECT
                    u.id AS user_id,
                    r.id AS id,
                    r.product_id AS product_id,
                    r.user_id AS user_id,
                    r.product_rating AS product_rating,
                    r.rating AS rating,
                    r.title AS title,
                    r.text AS text,
                    r.created_at AS created_at,
                    r.updated_at as updated_at
                FROM users AS u INNER JOIN reviews AS r ON u.id = r.user_id
                WHERE u.id = @userId
                LIMIT @reviewsLimit
                OFFSET @reviewsOffset
                """,
                param: new
                {
                    userId = query.Request.UserId,
                    reviewsLimit = query.Request.Pagination.PageSize,
                    reviewsOffset = (query.Request.Pagination.Page - 1) * query.Request.Pagination.PageSize,
                },
                splitOn:"id",
                map: (u, r) =>
                {
                    userReviewsDto ??= u;
                    userReviewsDto.Reviews.Add(r);

                    return userReviewsDto;
                });

        return userReviewsDto;
    }
}