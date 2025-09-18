using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Orders.Commands.CreateOrderCommand;
using Tea_Shop.Application.Orders.Commands.DeleteOrderCommand;
using Tea_Shop.Application.Orders.Commands.UpdateOrderCommand;
using Tea_Shop.Application.Orders.Queries.GetOrderByIdQuery;
using Tea_Shop.Application.Orders.Queries.GetOrderItemsQuery;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Domain.Orders;

namespace Tea_Shop.Orders;

[ApiController]
[Route("[controller]")]
public class OrdersController: ControllerBase
{
    [Authorize]
    [HttpGet("orders/{orderId:guid}")]
    public async Task<ActionResult<GetOrderResponseDto>> GetOrderById(
        [FromRoute] Guid orderId,
        [FromServices] IQueryHandler<GetOrderResponseDto?, GetOrderByIdQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetOrderByIdQuery(new GetOrderRequestDto(orderId));

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpGet("orders/{orderId:guid}/items")]
    public async Task<ActionResult<OrderItemDto[]>> GetOrderItems(
        [FromRoute] Guid orderId,
        [FromServices] IQueryHandler<OrderItemDto[], GetOrderItemQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetOrderItemQuery(new GetOrderItemsRequestDto(orderId));

        var orderItems = await handler.Handle(query, cancellationToken);

        return Ok(orderItems);
    }

    [Authorize]
    [HttpPost("orders")]
    public async Task<ActionResult<CreateOrderResponseDto>> CreateOrder(
        [FromBody] CreateOrderRequestDto request,
        [FromServices] ICommandHandler<CreateOrderResponseDto, CreateOrderCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateOrderCommand(request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [Authorize]
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

    [Authorize]
    [HttpDelete("orders/{orderId:guid}")]
    public async Task<IActionResult> DeleteOrder(
        [FromRoute] Guid orderId,
        [FromServices] ICommandHandler<DeleteOrderDto, DeleteOrderCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteOrderCommand(new DeleteOrderDto(orderId));

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(orderId);
    }
}