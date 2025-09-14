using Dapper;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Reviews;
using Tea_Shop.Contract.Users;

namespace Tea_Shop.Application.Users.Queries.GetUserReviewsQuery;

public class GetUserReviewsHandler:
    IQueryHandler<GetUserReviewsResponseDto?, GetUserReviewsQuery>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<GetUserReviewsHandler> _logger;

    public GetUserReviewsHandler(
        IDbConnectionFactory dbConnectionFactory,
        ILogger<GetUserReviewsHandler> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<GetUserReviewsResponseDto?> Handle(
        GetUserReviewsQuery query,
        CancellationToken cancellationToken)
    {
        var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);

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