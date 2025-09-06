using Dapper;
using Microsoft.Extensions.Logging;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Database;
using Tea_Shop.Contract.Comments;
using Tea_Shop.Contract.Reviews;

namespace Tea_Shop.Application.Reviews.Queries.GetReviewCommentsQuery;

public class GetReviewCommentsHandler:
    IQueryHandler<GetReviewCommentsResponseDto, GetReviewCommentsQuery>
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<GetReviewCommentsHandler> _logger;

    public GetReviewCommentsHandler(
        IDbConnectionFactory connectionFactory,
        ILogger<GetReviewCommentsHandler> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<GetReviewCommentsResponseDto> Handle(
        GetReviewCommentsQuery query,
        CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        var reviewComments = await connection.QueryAsync<CommentDto>(
            """
            select
                id,
                user_id,
                text,
                rating,
                review_id,
                parent_id,
                created_at,
                updated_at
            from comments
            where review_id = @reviewId
            """,
            param: new
            {
                reviewId = query.Request.ReviewId
            });

        var response = new GetReviewCommentsResponseDto()
        {
            ReviewId = query.Request.ReviewId,
            Comments = reviewComments.ToList(),
        };

        return response;
    }
}