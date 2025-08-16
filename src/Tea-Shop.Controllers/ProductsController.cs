using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Contract.Products;

namespace Tea_Shop;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetProduct(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        return Ok("Get product");
    }

    [HttpGet("{productId:guid}/ingrindients")]
    public async Task<IActionResult> GetProductIngrindients(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        return Ok("Get product's ingredients");
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(
        CreateProductRequestDto request,
        CancellationToken cancellationToken)
    {
        return Ok("Created product");
    }

    [HttpPatch("{productId:guid}/price")]
    public async Task<IActionResult> UpdateProductPrice(
        UpdateProductPriceRequestDto request,
        CancellationToken cancellationToken)
    {
        return Ok("Updated product's price by id");
    }

    [HttpPatch("{productId:guid}/amount")]
    public async Task<IActionResult> UpdateProductAmount(
        UpdateProductAmountRequestDto request,
        CancellationToken cancellationToken)
    {
        return Ok("Updated product's amount by id");
    }

    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> DeleteProduct(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        return Ok("Deleted product");
    }



    [HttpGet("orders/{orderId:guid}")]
    public async Task<IActionResult> GetOrder(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        return Ok("Get order");
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
        CreateOrderRequestDto request,
        CancellationToken cancellationToken)
    {
        return Ok("Created product");
    }

    [HttpPatch("orders/{productId:guid}/price")]
    public async Task<IActionResult> UpdateOrderItems(
        UpdateOrderItemsRequestDto request,
        CancellationToken cancellationToken)
    {
        return Ok("Updated order's items by id");
    }

    [HttpPatch("orders/{productId:guid}/price")]
    public async Task<IActionResult> UpdateOrderStatus(
        UpdateOrderStatusRequestDto request,
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