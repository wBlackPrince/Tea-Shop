using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Orders;
using Tea_Shop.Application.Orders.Commands;
using Tea_Shop.Application.Orders.Queries;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Domain.Orders;

namespace Tea_Shop.Orders;

[ApiController]
[Route("[controller]")]
public class OrdersController: ControllerBase
{
    [HttpGet("orders/{orderId:guid}")]
    public async Task<ActionResult<GetOrderResponseDto>> GetOrderById(
        [FromRoute] Guid orderId,
        [FromServices] GetOrderByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(orderId, cancellationToken);

        return Ok(result);
    }

    [HttpGet("orders/{orderId:guid}/items")]
    public async Task<IActionResult> GetOrderItems(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        return Ok("Get order's items");
    }

    [HttpPost("orders")]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderRequestDto request,
        [FromServices] CreateOrderHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok($"Created order with id {result.Value}");
    }

    [HttpPatch("orders/{orderId:guid}")]
    public async Task<IActionResult> UpdateOrder(
        [FromRoute] Guid orderId,
        [FromBody] JsonPatchDocument<Order> orderUpdates,
        [FromServices] UpdateOrderHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(orderId, orderUpdates, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok($"Updated order by id {result.Value}");
    }


    [HttpDelete("orders/{orderId:guid}")]
    public async Task<IActionResult> DeleteOrder(
        [FromRoute] Guid orderId,
        [FromServices] DeleteOrderHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(orderId, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(orderId);
    }
}