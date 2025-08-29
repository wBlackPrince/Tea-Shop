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
    public async Task<ActionResult<GetProductResponseDto>> GetProductById(
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

    [HttpGet]
    public async Task<ActionResult<GetProductResponseDto[]>> GetProductsByTag(
        [FromQuery]Guid tagId,
        CancellationToken cancellationToken)
    {
        var productsResult = await _productService.GetProductsByTag(tagId, cancellationToken);

        if (productsResult.IsFailure)
        {
            return NotFound(productsResult.Error);
        }

        return Ok(productsResult.Value);
    }

    [HttpGet("{productId:guid}/ingridients")]
    public async Task<ActionResult<GetIngrendientsResponseDto[]>> GetProductsIngredients(
        [FromRoute]Guid productId,
        CancellationToken cancellationToken)
    {
        var result = await _productService.GetProductIngredients(
            productId,
            cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<CreateProductResponseDto>> Create(
        [FromBody]CreateProductRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = await _productService.CreateProduct(request, cancellationToken);
        return Ok(response);
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
}