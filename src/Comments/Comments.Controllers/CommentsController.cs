using Comments.Application.Commands.CreateCommentCommand;
using Comments.Application.Commands.DeleteCommentCommand;
using Comments.Application.Commands.UpdateCommentCommand;
using Comments.Application.Queries.GetCommentByIdQuery;
using Comments.Application.Queries.GetCommentChildCommentsQuery;
using Comments.Contracts;
using Comments.Contracts.Dtos;
using Comments.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Shared.Abstractions;

namespace Comments.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController: ControllerBase
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


    [HttpGet("{commentId:guid}/comments")]
    public async Task<ActionResult<CommentDto>> GetChildCommentById(
        [FromRoute] Guid commentId,
        [FromServices]IQueryHandler<GetChildCommentsResponseDto, GetCommentChildCommentsQuery> handler,
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
}