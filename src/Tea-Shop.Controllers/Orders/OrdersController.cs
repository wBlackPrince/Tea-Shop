using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Orders;
using Tea_Shop.Contract.Orders;

namespace Tea_Shop.Orders;

[ApiController]
[Route("[controller]")]
public class OrdersController: ControllerBase
{
    private readonly IOrdersService _ordersService;

    public OrdersController(IOrdersService orderService)
    {
        _ordersService = orderService;
    }

    [HttpGet("orders/{orderId:guid}")]
    public async Task<ActionResult<GetOrderResponseDto>> GetOrderById(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        var result = await _ordersService.GetOrderById(orderId, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }


    [HttpGet("orders/{orderId:guid}")]
    public async Task<IActionResult> GetOrderItems(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        return Ok("Get order's items");
    }

    [HttpPost("orders")]
    public async Task<IActionResult> CreateOrder(
        [FromBody]CreateOrderRequestDto request,
        CancellationToken cancellationToken)
    {
        var orderId = await _ordersService.CreateOrder(request, cancellationToken);
        return Ok($"Created order with id {orderId}");
    }

    [HttpPatch("orders/{productId:guid}/price")]
    public async Task<IActionResult> UpdateOrderItems(
        [FromBody]UpdateOrderItemsRequestDto request,
        CancellationToken cancellationToken)
    {
        return Ok("Updated order's items by id");
    }

    [HttpPatch("orders/{productId:guid}/price")]
    public async Task<IActionResult> UpdateOrderStatus(
        [FromBody]UpdateOrderStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        return Ok("Updated order's status by id");
    }

    [HttpDelete("orders/{orderId:guid}")]
    public async Task<IActionResult> DeleteOrder(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        var result = await _ordersService.DeleteOrder(orderId, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(orderId);
    }
}