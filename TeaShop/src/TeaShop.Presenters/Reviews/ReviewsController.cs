using Microsoft.AspNetCore.Mvc;
using TeaShop.Contract.Reviews;
using TeaShop.Contract.Tags;

namespace TeaShop.Presenters.Reviews;

[ApiController]
[Route("[controller]")]
public class ReviewsController: ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateReview(
        [FromBody] CreateReviewDto request,
        CancellationToken cancellationToken)
    {
        return Ok("Created review");
    }

    [HttpPut("{reviewId:guid}")]
    public async Task<IActionResult> UpdateReview(
        [FromRoute] Guid reviewId,
        [FromBody] UpdateReviewDto request,
        CancellationToken cancellationToken)
    {
        return Ok("Updated review");
    }

    [HttpPut("{reviewId:guid}/rating")]
    public async Task<IActionResult> UpdateReviewRating(
        [FromRoute] Guid reviewId,
        [FromBody] UpdateReviewRatingDto request,
        CancellationToken cancellationToken)
    {
        return Ok($"Updated review rating {reviewId}");
    }

    [HttpGet("{reviewId:guid}")]
    public async Task<IActionResult> GetReview(
        [FromRoute] Guid reviewId,
        CancellationToken cancellationToken)
    {
        return Ok("Get review");
    }

    [HttpDelete("{reviewId:guid}")]
    public async Task<IActionResult> DeleteReview(
        [FromRoute] Guid reviewId,
        CancellationToken cancellationToken)
    {
        return Ok("Deleted review");
    }
}