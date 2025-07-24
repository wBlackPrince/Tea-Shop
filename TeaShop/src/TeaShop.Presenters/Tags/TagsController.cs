using Microsoft.AspNetCore.Mvc;
using TeaShop.Contract.Tags;

namespace TeaShop.Presenters.Tags;

[ApiController]
[Route("[controller]")]
public class TagsController: ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTag(
        [FromBody] CreateTagDto request,
        CancellationToken cancellationToken)
    {
        return Ok("Created Tag");
    }

    [HttpGet("/popular")]
    public async Task<IActionResult> GetPopularTags(CancellationToken cancellationToken)
    {
        return Ok("Get popular Tags");
    }
}
