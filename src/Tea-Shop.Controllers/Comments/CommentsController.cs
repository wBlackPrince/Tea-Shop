using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Comments.Commands;
using Tea_Shop.Application.Comments.Commands.CreateCommentCommand;
using Tea_Shop.Application.Comments.Commands.DeleteCommentCommand;
using Tea_Shop.Application.Comments.Commands.UpdateCommentCommand;
using Tea_Shop.Application.Comments.Queries;
using Tea_Shop.Contract.Comments;
using Tea_Shop.Domain.Comments;

namespace Tea_Shop.Comments;

[ApiController]
[Route("[controller]")]
public class CommentsController: ControllerBase
{
    [HttpGet("{commentId:guid}")]
    public async Task<IActionResult> GetCommentById(
        [FromRoute] Guid commentId,
        [FromServices] GetCommentByIdHandler handler,
        CancellationToken cancellationToken)
    {
        await handler.Handle(commentId, cancellationToken);

        return Ok(commentId);
    }

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

    [HttpDelete("{commentId:guid}")]
    public async Task<IActionResult> DeleteComment(
        [FromRoute] Guid commentId,
        [FromServices] DeleteCommentHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(commentId, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(commentId);
    }
}