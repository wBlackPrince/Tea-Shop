using Microsoft.AspNetCore.Mvc;
using TeaShop.Application.Products;
using TeaShop.Contract.Products;

namespace TeaShop.Presenters.Products;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateProduct(
        [FromBody] CreateProductDto request,
        CancellationToken cancellationToken)
    {
        Guid productId = await _service.Create(request, cancellationToken);

        return this.Ok($"Created product {productId}");
    }


    [HttpPut("{productId:guid}")]
    public async Task<IActionResult> UpdateProduct(
        [FromRoute] Guid id,
        [FromBody] UpdateProductDto request,
        CancellationToken cancellationToken)
    {
        return this.Ok("Updated product");
    }

    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> DeleteProduct(
            [FromRoute] Guid productId,
            CancellationToken cancellationToken)
    {
        return this.Ok("Deleted product");
    }

    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetProductById(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        return this.Ok("Get product by id");
    }

    [HttpGet]
    public async Task<IActionResult> GetProductByTag([FromQuery] Guid tagId, CancellationToken cancellationToken)
    {
        return this.Ok("Get product by tag");
    }

    [HttpGet("seasonal/spring")]
    public async Task<IActionResult> GetSpringProducts(CancellationToken cancellationToken)
    {
        return this.Ok("Spring products");
    }

    [HttpGet("seasonal/summer")]
    public async Task<IActionResult> GetSummerProducts(CancellationToken cancellationToken)
    {
        return this.Ok("Summer products");
    }

    [HttpGet("seasonal/autumn")]
    public async Task<IActionResult> GetAutumnProducts(CancellationToken cancellationToken)
    {
        return this.Ok("Autumn products");
    }

    [HttpGet("seasonal/winter")]
    public async Task<IActionResult> GetWinterProducts(CancellationToken cancellationToken)
    {
        return this.Ok("Winter products");
    }
}


