using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Orders.Commands;
using Tea_Shop.Application.Reviews;
using Tea_Shop.Application.Reviews.Commands;
using Tea_Shop.Application.Reviews.Queries;
using Tea_Shop.Contract.Reviews;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Reviews;

namespace Tea_Shop.Reviews;

[ApiController]
[Route("[controller]")]
public class ReviewsController: ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetReviewById(
        [FromRoute] Guid reviewId,
        [FromServices] GetReviewByIdHandler handler,
        CancellationToken cancellationToken)
    {
        await handler.Handle(reviewId, cancellationToken);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateReview(
        [FromBody] CreateReviewRequestDto request,
        [FromServices] CreateReviewHandler handler,
        CancellationToken cancellationToken)
    {
        await handler.Handle(request, cancellationToken);
        return Ok();
    }

    [HttpPatch("reviews/{reviewId:guid}")]
    public async Task<IActionResult> UpdateReview(
        [FromRoute] Guid reviewId,
        [FromBody] JsonPatchDocument<Review> reviewUpdates,
        [FromServices] UpdateReviewHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(reviewId, reviewUpdates, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok($"Updated review by id {result.Value}");
    }

    [HttpDelete("{reviewId:guid}")]
    public async Task<IActionResult> DeleteReview(
        [FromRoute] Guid reviewId,
        [FromServices] DeleteReviewHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(reviewId, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }
}