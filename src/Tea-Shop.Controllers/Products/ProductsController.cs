using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Products;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;

namespace Tea_Shop.Products;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductsService _productService;

    public ProductsController(IProductsService productService)
    {
        _productService = productService;
    }

    [HttpGet("{productId:guid}")]
    public async Task<ActionResult<GetProductResponseDto>> GetById(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        var getResult = await _productService.GetProductById(
            productId,
            cancellationToken);

        if (getResult.IsFailure)
        {
            return NotFound(getResult.Error);
        }

        return Ok(getResult.Value);
    }

    [HttpGet("{productId:guid}/ingrindients")]
    public async Task<IActionResult> GetProductIngrindients(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        return Ok("Get product's ingredients");
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody]CreateProductRequestDto request,
        CancellationToken cancellationToken)
    {
        var productId = await _productService.CreateProduct(request, cancellationToken);
        return Ok(productId);
    }

    [HttpPatch("{productId:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid productId,
        [FromBody] JsonPatchDocument<Product> productUpdates,
        CancellationToken cancellationToken)
    {
        var updateResult = await _productService.UpdateProduct(productId, productUpdates, cancellationToken);

        if (updateResult.IsFailure)
        {
            return NotFound(updateResult.Error);
        }

        return Ok(updateResult.Value);
    }

    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> DeleteProduct(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        var deleteResult = await _productService.DeleteProduct(productId, cancellationToken);

        if (deleteResult.IsFailure)
        {
            return NotFound(deleteResult.Error);
        }

        return Ok(productId);
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
        var orderId = await _productService.CreateOrder(request, cancellationToken);
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