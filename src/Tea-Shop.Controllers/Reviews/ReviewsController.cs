using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Reviews;
using Tea_Shop.Application.Reviews.Commands;
using Tea_Shop.Application.Reviews.Queries;
using Tea_Shop.Contract.Reviews;

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