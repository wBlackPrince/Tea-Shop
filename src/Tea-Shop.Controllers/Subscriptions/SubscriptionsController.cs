using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Subscriptions.Commands.CreateKitCommand;
using Tea_Shop.Application.Subscriptions.Queries.GetKitByIdQuery;
using Tea_Shop.Contract.Subscriptions;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Subscriptions;


[ApiController]
[Route("[controller]")]
public class SubscriptionsController: ControllerBase
{
    [Authorize(Roles = $"{Role.AdminRoleName},{Role.UserRoleName}")]
    [HttpGet("kits/{kitId:guid}")]
    public async Task<ActionResult<KitDto>> GetKitById(
        [FromRoute] Guid kitId,
        [FromServices]IQueryHandler<KitDto, GetKitByIdQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetKitByIdQuery(new KitWithOnlyIdDto(kitId));

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }

    [Authorize(Roles = Role.AdminRoleName)]
    [HttpPost("kits")]
    public async Task<ActionResult<KitDto>> CreateKit(
        [FromBody] CreateKitRequestDto request,
        [FromServices]ICommandHandler<KitDto, CreateKitCommand> handler,
        CancellationToken cancellationToken)
    {
        var query = new CreateKitCommand(request);

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }
}