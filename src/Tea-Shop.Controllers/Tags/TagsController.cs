using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Tags;
using Tea_Shop.Contract.Tags;
using Tea_Shop.Shared;

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


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTag(
        [FromBody]CreateTagRequestDto request,
        CancellationToken cancellationToken)
    {
        Guid tagId = await _tagsService.CreateTag(request, cancellationToken);

        return Ok(tagId);
    }

    [Authorize]
    [HttpDelete("{tagId:guid}")]
    public async Task<ActionResult<Guid>> DeleteTag(
        [FromRoute] Guid tagId,
        CancellationToken cancellationToken)
    {
        var result = await _tagsService.Delete(tagId, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(tagId);
    }
}