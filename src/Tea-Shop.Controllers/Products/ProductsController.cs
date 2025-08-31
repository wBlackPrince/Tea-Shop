using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Products;
using Tea_Shop.Application.Products.Commands;
using Tea_Shop.Application.Products.Queries;
using Tea_Shop.Contract.Products;
using Tea_Shop.Domain.Products;

namespace Tea_Shop.Products;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet("{productId:guid}")]
    public async Task<ActionResult<GetProductByIdResponseDto>> GetProductById(
        [FromServices] GetProductByIdHandler handler,
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        var getResult = await handler.Handle(
            new GetProductByIdRequestDto(productId),
            cancellationToken);

        return Ok(getResult);
    }

    [HttpGet]
    public async Task<ActionResult<GetProductByIdResponseDto[]>> GetProductsByTag(
        [FromServices] GetProductsByTagHandler handler,
        [FromQuery] Guid tagId,
        CancellationToken cancellationToken)
    {
        var productsResult = await handler.Handle(
            new GetProductsByTagRequestDto(tagId),
            cancellationToken);

        return Ok(productsResult);
    }

    [HttpGet("{productId:guid}/ingridients")]
    public async Task<ActionResult<GetIngrendientsResponseDto[]>> GetProductsIngredients(
        [FromServices] GetProductIngredientsHandler handler,
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new GetProductIngridientsRequestDto(productId),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CreateProductResponseDto>> Create(
        [FromServices] CreateProductHandler handler,
        [FromBody] CreateProductRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(request, cancellationToken);
        return Ok(response);
    }

    [HttpPatch("{productId:guid}")]
    public async Task<IActionResult> Update(
        [FromServices] UpdateProductHandler handler,
        [FromRoute] Guid productId,
        [FromBody] JsonPatchDocument<Product> productUpdates,
        CancellationToken cancellationToken)
    {
        var updateResult = await handler.Handle(productId, productUpdates, cancellationToken);

        if (updateResult.IsFailure)
        {
            return NotFound(updateResult.Error);
        }

        return Ok(updateResult.Value);
    }

    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> DeleteProduct(
        [FromServices] DeleteProductHandler handler,
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        var deleteResult = await handler.Handle(productId, cancellationToken);

        if (deleteResult.IsFailure)
        {
            return NotFound(deleteResult.Error);
        }

        return Ok(productId);
    }
}