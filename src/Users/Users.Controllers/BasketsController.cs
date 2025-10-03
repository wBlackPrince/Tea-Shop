using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Contracts.Dtos;
using Shared.Abstractions;
using Users.Application.Commands.AddBasketItemCommand;
using Users.Application.Commands.RemoveBasketItemCommand;
using Users.Contracts.Dtos;

namespace Users.Controllers;

[ApiController]
[Route("[controller]")]
public class BasketsController: ControllerBase
{
    [Authorize]
    [HttpPost("basket-items")]
    public async Task<ActionResult<CreateOrderResponseDto>> AddBasketItem(
        [FromBody] AddBasketItemDto request,
        [FromServices] ICommandHandler<AddBasketItemDto?, AddBasketItemCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new AddBasketItemCommand(request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [Authorize]
    [HttpDelete("basket-items")]
    public async Task<ActionResult<CreateOrderResponseDto>> RemoveBasketItem(
        [FromBody] RemoveBasketItemDto request,
        [FromServices] ICommandHandler<RemoveBasketItemDto?, RemoveBasketItemCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new RemoveBasketItemCommand(request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}