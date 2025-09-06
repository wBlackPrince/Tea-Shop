using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Orders.Commands;
using Tea_Shop.Application.Reviews;
using Tea_Shop.Application.Reviews.Commands;
using Tea_Shop.Application.Reviews.Commands.CreateReviewCommand;
using Tea_Shop.Application.Reviews.Commands.DeleteReviewCommand;
using Tea_Shop.Application.Reviews.Commands.UpdateReviewCommand;
using Tea_Shop.Application.Reviews.Queries;
using Tea_Shop.Application.Reviews.Queries.GetReviewByIdQuery;
using Tea_Shop.Application.Reviews.Queries.GetReviewCommentsQuery;
using Tea_Shop.Contract.Reviews;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Reviews;

namespace Tea_Shop.Reviews;

[ApiController]
[Route("[controller]")]
public class ReviewsController: ControllerBase
{
    [HttpGet("{reviewId:guid}")]
    public async Task<ActionResult<GetReviewResponseDto>> GetReviewById(
        [FromRoute] Guid reviewId,
        [FromServices] IQueryHandler<GetReviewResponseDto?, GetReviewByIdQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetReviewByIdQuery(new GetReviewRequestDto(reviewId));

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{reviewId:guid}/comments")]
    public async Task<ActionResult<GetReviewCommentsResponseDto>> GetReviewComments(
        [FromRoute] Guid reviewId,
        [FromServices] IQueryHandler<GetReviewCommentsResponseDto, GetReviewCommentsQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetReviewCommentsQuery(new GetReviewRequestDto(reviewId));

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateReview(
        [FromBody] CreateReviewRequestDto request,
        [FromServices] CreateReviewHandler handler,
        CancellationToken cancellationToken)
    {
        await handler.Handle(
            new CreateReviewCommand(request),
            cancellationToken);
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
        [FromServices] ICommandHandler<DeleteReviewDto, DeleteReviewCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteReviewCommand(new DeleteReviewDto(reviewId));

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }
}