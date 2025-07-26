using Microsoft.AspNetCore.Mvc;
using TeaShop.Contract.Comments;

namespace TeaShop.Presenters.Comments;

[ApiController]
[Route("[controller]")]
public class CommentsController: ControllerBase
{
    [HttpPost("{commentId:guid}/replies")]
    public async Task<IActionResult> CreateComment(
        [FromRoute] Guid commentId,
        [FromBody] CreateCommentDto request,
        CancellationToken cancellationToken)
    {
        return this.Ok($"Created a comment by user {request.UserId}");
    }

    [HttpPut("{commentId:guid}")]
    public async Task<IActionResult> UpdateComment(
        [FromRoute] Guid commentId,
        [FromBody] UpdateCommentDto request,
        CancellationToken cancellationToken)
    {
        return this.Ok($"Updated comment with id {commentId}");
    }

    [HttpPut("{commentId:guid}/rating")]
    public async Task<IActionResult> UpdateCommentRating(
        [FromRoute] Guid commentId,
        [FromBody] UpdateCommentRatingDto request,
        CancellationToken cancellationToken)
    {
        return this.Ok($"Updated comment's rating by user {commentId}");
    }

    [HttpGet("{commentId:guid}")]
    public async Task<IActionResult> GetCommentById(
        [FromRoute] Guid commentId,
        CancellationToken cancellationToken)
    {
        return this.Ok($"Get comment with id {commentId}");
    }

    [HttpDelete("{commentId:guid}")]
    public async Task<IActionResult> DeleteComment(
        [FromRoute] Guid commentId,
        CancellationToken cancellationToken)
    {
        return this.Ok($"Deleted comment with id {commentId}");
    }
}