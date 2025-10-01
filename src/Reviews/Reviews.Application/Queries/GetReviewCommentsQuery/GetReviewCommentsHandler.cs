using Microsoft.Extensions.Logging;
using Reviews.Contracts;
using Shared.Abstractions;
using Shared.Database;

namespace Reviews.Application.Queries.GetReviewCommentsQuery;

public class GetReviewCommentsHandler(
    IDbConnectionFactory connectionFactory,
    ILogger<GetReviewCommentsHandler> logger):
    IQueryHandler<GetReviewCommentsResponseDto, GetReviewCommentsQuery>
{
    public async Task<GetReviewCommentsResponseDto> Handle(
        GetReviewCommentsQuery query,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling {handler}", nameof(GetReviewCommentsHandler));

        using var connection = await connectionFactory.CreateConnectionAsync(cancellationToken);

        var reviewComments = (await connection.QueryAsync<CommentDto>(
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
            param: new { reviewId = query.Request.ReviewId }))
            .ToList();

        if (reviewComments.Count == 0)
        {
            logger.LogWarning(
                "Comments from review with id {reviewId} not found",
                query.Request.ReviewId);
        }

        var response = new GetReviewCommentsResponseDto()
        {
            ReviewId = query.Request.ReviewId,
            Comments = reviewComments,
        };

        logger.LogDebug("Get comments from review with id {reviewId}", query.Request.ReviewId);

        return response;
    }
}