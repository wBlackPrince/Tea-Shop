using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Orders;
using Tea_Shop.Contract.Products;

namespace Tea_Shop.OrdersController;

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
    public async Task<IActionResult> GetOrder(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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
        return Ok("Deleted order");
    }
}