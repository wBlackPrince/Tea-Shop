using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Comments;
using Tea_Shop.Application.Reviews;
using Tea_Shop.Contract.Comments;
using Tea_Shop.Contract.Reviews;

namespace Tea_Shop.Comments;

[ApiController]
[Route("[controller]")]
public class CommentsController: ControllerBase
{
    private readonly ICommentsService _reviewsService;

    public CommentsController(ICommentsService reviewsService)
    {
        _reviewsService = reviewsService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateComment(
        [FromBody] CreateCommentRequestDto request,
        CancellationToken cancellationToken)
    {
        await _reviewsService.CreateComment(request, cancellationToken);
        return Ok();
    }
}