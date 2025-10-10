using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Social.Commands.CreateCommentCommand;
using Tea_Shop.Application.Social.Commands.CreateReviewCommand;
using Tea_Shop.Application.Social.Commands.DeleteCommentCommand;
using Tea_Shop.Application.Social.Commands.DeleteReviewCommand;
using Tea_Shop.Application.Social.Commands.UpdateCommentCommand;
using Tea_Shop.Application.Social.Commands.UpdateReviewCommand;
using Tea_Shop.Application.Social.Queries.GetCommentByIdQuery;
using Tea_Shop.Application.Social.Queries.GetCommentChildCommentsQuery;
using Tea_Shop.Application.Social.Queries.GetDescendantsQuery;
using Tea_Shop.Application.Social.Queries.GetHierarchyQuery;
using Tea_Shop.Application.Social.Queries.GetNeighboursQuery;
using Tea_Shop.Application.Social.Queries.GetReviewByIdQuery;
using Tea_Shop.Application.Social.Queries.GetReviewCommentsQuery;
using Tea_Shop.Contract.Social;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Social;

[Authorize(Roles = Role.AdminRoleName)]
[Authorize(Roles = Role.UserRoleName)]
[ApiController]
[Route("[controller]")]
public class SocialController: ControllerBase
{
    [HttpGet("{commentId:guid}")]
    public async Task<ActionResult<CommentDto>> GetCommentById(
        [FromRoute] Guid commentId,
        [FromServices]IQueryHandler<CommentDto, GetCommentByIdQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetCommentByIdQuery(new CommentWithOnlyIdDto(commentId));

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{commentId:guid}/hierarchy")]
    public async Task<ActionResult<CommentDto>> GetCommentHierarchy(
        [FromRoute] Guid commentId,
        [FromServices]IQueryHandler<CommentsResponseDto, GetHierarchyQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetHierarchyQuery(new CommentWithOnlyIdDto(commentId));

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{commentId:guid}/neighbours")]
    public async Task<ActionResult<CommentDto>> GetCommentNeighbours(
        [FromRoute] Guid commentId,
        [FromServices]IQueryHandler<CommentsResponseDto, GetNeighboursQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetNeighboursQuery(new CommentWithOnlyIdDto(commentId));

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{commentId:guid}/descendants")]
    public async Task<ActionResult<CommentDto>> GetCommentDescendants(
        [FromRoute] Guid commentId,
        [FromQuery] int depth,
        [FromServices]IQueryHandler<CommentsResponseDto, GetDescendantsQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetDescendantsQuery(new GetDescendantsRequestDto(commentId, depth));

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }


    [HttpGet("{commentId:guid}/comments")]
    public async Task<ActionResult<CommentDto>> GetChildCommentById(
        [FromRoute] Guid commentId,
        [FromServices]IQueryHandler<CommentsResponseDto, GetCommentChildCommentsQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetCommentChildCommentsQuery(new CommentWithOnlyIdDto(commentId));

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CreateCommentResponseDto>> CreateComment(
        [FromBody] CreateCommentRequestDto request,
        [FromServices] ICommandHandler<CreateCommentResponseDto, CreateCommentCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateCommentCommand(request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [Authorize]
    [HttpPatch("{commentId:guid}")]
    public async Task<IActionResult> UpdateComment(
        [FromRoute] Guid commentId,
        [FromServices] UpdateCommentHandler handler,
        [FromBody] JsonPatchDocument<Comment> commentUpdates,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(commentId, commentUpdates, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [Authorize]
    [HttpDelete("{commentId:guid}")]
    public async Task<ActionResult<CommentWithOnlyIdDto>> DeleteComment(
        [FromRoute] Guid commentId,
        [FromServices] ICommandHandler<CommentWithOnlyIdDto, DeleteCommentCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCommentCommand(new CommentWithOnlyIdDto(commentId));

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(commentId);
    }

    [HttpGet("reviews/{reviewId:guid}")]
    public async Task<ActionResult<GetReviewResponseDto>> GetReviewById(
        [FromRoute] Guid reviewId,
        [FromServices] IQueryHandler<GetReviewResponseDto?, GetReviewByIdQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetReviewByIdQuery(new GetReviewRequestDto(reviewId));

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("reviews/{reviewId:guid}/comments")]
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
    [HttpPost("reviews")]
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

    [Authorize]
    [HttpDelete("reviews/{reviewId:guid}")]
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