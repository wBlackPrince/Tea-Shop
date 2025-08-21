using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Tags;
using Tea_Shop.Contract.Tags;

namespace Tea_Shop.Tags;

[ApiController]
[Route("[controller]")]
public class TagsController: ControllerBase
{
    private readonly ITagsService _tagsService;

    public TagsController(ITagsService tagsService)
    {
        _tagsService = tagsService;
    }


    [HttpPost]
    public async Task<IActionResult> CreateTag(
        [FromBody]CreateTagRequestDto request,
        CancellationToken cancellationToken)
    {
        await _tagsService.CreateTag(request, cancellationToken);

        return Ok();
    }
}