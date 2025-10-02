using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Reviews.Application.Commands.CreateReviewCommand;
using Reviews.Application.Commands.DeleteReviewCommand;
using Reviews.Application.Commands.UpdateReviewCommand;
using Reviews.Application.Queries.GetReviewByIdQuery;
using Reviews.Application.Queries.GetReviewCommentsQuery;
using Reviews.Contracts;
using Reviews.Contracts.Dtos;
using Reviews.Domain;
using Shared.Abstractions;

namespace Reviews.Controllers;

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

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateReview(
        [FromBody] CreateReviewRequestDto request,
        [FromServices] ICommandHandler<CreateReviewResponseDto, CreateReviewCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.Handle(
            new CreateReviewCommand(request),
            cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPatch("{reviewId:guid}")]
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

    [Authorize]
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