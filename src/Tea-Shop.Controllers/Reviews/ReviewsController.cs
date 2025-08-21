using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Reviews;
using Tea_Shop.Contract.Reviews;

namespace Tea_Shop.Reviews;

[ApiController]
[Route("[controller]")]
public class ReviewsController: ControllerBase
{
    private readonly IReviewsService _reviewsService;

    public ReviewsController(IReviewsService reviewsService)
    {
        _reviewsService = reviewsService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReview(
        [FromBody] CreateReviewRequestDto request,
        CancellationToken cancellationToken)
    {
        await _reviewsService.CreateReview(request, cancellationToken);
        return Ok();
    }
}