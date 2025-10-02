using Baskets.Application.Commands.AddBasketItemCommand;
using Baskets.Application.Commands.RemoveBasketItemCommand;
using Baskets.Contracts;
using Baskets.Contracts.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Contracts;
using Orders.Contracts.Dtos;
using Shared.Abstractions;

namespace Baskets.Controllers;

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