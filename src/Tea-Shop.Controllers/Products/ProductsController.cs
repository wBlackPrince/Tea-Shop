using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Products.Commands.CreateProductCommand;
using Tea_Shop.Application.Products.Commands.DeleteProductCommand;
using Tea_Shop.Application.Products.Commands.UpdatePreparationDescription;
using Tea_Shop.Application.Products.Commands.UpdatePreparationTime;
using Tea_Shop.Application.Products.Commands.UpdateProductCommand;
using Tea_Shop.Application.Products.Commands.UpdateProductIngredients;
using Tea_Shop.Application.Products.Queries.GetPopularProductsQuery;
using Tea_Shop.Application.Products.Queries.GetProductByIdQuery;
using Tea_Shop.Application.Products.Queries.GetProductIngredientsQuery;
using Tea_Shop.Application.Products.Queries.GetProductReviews;
using Tea_Shop.Application.Products.Queries.GetProductsQuery;
using Tea_Shop.Application.Products.Queries.GetSimilarProductsQuery;
using Tea_Shop.Contract.Products;
using Tea_Shop.Contract.Reviews;
using Tea_Shop.Domain.Products;

namespace Tea_Shop.Products;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet("{productId:guid}")]
    public async Task<ActionResult<GetProductDto?>> GetProductById(
        [FromServices] IQueryHandler<GetProductDto, GetProductByIdQuery> handler,
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(new ProductWithOnlyIdDto(productId));

        var getResult = await handler.Handle(query, cancellationToken);

        return Ok(getResult);
    }

    [HttpGet]
    public async Task<ActionResult<GetProductDto[]>> GetProducts(
        [FromServices] IQueryHandler<GetProductsResponseDto, GetProductsQuery> handler,
        [FromQuery] GetProductsRequestDto request,
        CancellationToken cancellationToken)
    {
        var query = new GetProductsQuery(request);

        var products = await handler.Handle(query, cancellationToken);

        return Ok(products);
    }

    [HttpGet("{productId:guid}/ingridients")]
    public async Task<ActionResult<GetIngrendientsResponseDto[]>> GetProductsIngredients(
        [FromServices] IQueryHandler<GetIngrendientsResponseDto[], GetProductsIngredientsQuery> handler,
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        var query = new GetProductsIngredientsQuery(new GetProductIngridientsRequestDto(productId));

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{productId:guid}/reviews")]
    public async Task<ActionResult<GetReviewResponseDto[]>> GetProductReviews(
        [FromServices] IQueryHandler<GetReviewResponseDto[], GetProductReviewsQuery> handler,
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        var query = new GetProductReviewsQuery(productId);

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("popular")]
    public async Task<ActionResult<GetReviewResponseDto[]>> GetPopularProducts(
        [FromServices] IQueryHandler<
            GetPopularProductsResponseDto[],
            GetPopularProductsQuery> handler,
        [FromQuery] int popularProductsCount,
        [FromQuery] DateTime startSeasonDate,
        [FromQuery] DateTime endSeasonDate,
        CancellationToken cancellationToken)
    {
        var request = new GetPopularProductRequestDto()
        {
            PopularProductsCount = popularProductsCount,
            StartSeasonDate = startSeasonDate,
            EndSeasonDate = endSeasonDate,
        };

        var query = new GetPopularProductsQuery(request);

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{productId:guid}/similar")]
    public async Task<ActionResult<GetReviewResponseDto[]>> GetSimilarProducts(
        [FromRoute] Guid productId,
        [FromServices] IQueryHandler<
            GetSimilarProductResponseDto[],
            GetSimilarProductsQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetSimilarProductsQuery(new ProductWithOnlyIdDto(productId));

        var result = await handler.Handle(query, cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CreateProductResponseDto>> Create(
        [FromServices] ICommandHandler<CreateProductResponseDto, CreateProductCommand> handler,
        [FromBody] CreateProductRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result);
    }

    [Authorize]
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

    [Authorize]
    [HttpPatch("{productId:guid}/ingredients")]
    public async Task<IActionResult> UpdateIngredients(
        [FromServices] ICommandHandler<ProductWithOnlyIdDto, UpdateProductIngredientsCommand> handler,
        [FromRoute] Guid productId,
        [FromBody] GetIngrendientsResponseDto[] productUpdates,
        CancellationToken cancellationToken)
    {
        var command = new UpdateProductIngredientsCommand(productId, new IngridientsDto(productUpdates));
        var updateResult = await handler.Handle(command, cancellationToken);

        if (updateResult.IsFailure)
        {
            return NotFound(updateResult.Error);
        }

        return Ok(updateResult.Value);
    }

    [Authorize]
    [HttpPatch("preparation-description")]
    public async Task<IActionResult> UpdatePreparationDescription(
        [FromServices] ICommandHandler<ProductWithOnlyIdDto, UpdatePreparationDescriptionCommand> handler,
        [FromBody] UpdatePreparationDescriptionRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePreparationDescriptionCommand(request);
        var updateResult = await handler.Handle(command, cancellationToken);

        if (updateResult.IsFailure)
        {
            return NotFound(updateResult.Error);
        }

        return Ok(updateResult.Value);
    }


    [Authorize]
    [HttpPatch("preparation-time")]
    public async Task<IActionResult> UpdatePreparationTime(
        [FromServices] ICommandHandler<ProductWithOnlyIdDto, UpdatePreparationTimeCommand> handler,
        [FromBody] UpdatePreparationTimeRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePreparationTimeCommand(request);
        var updateResult = await handler.Handle(command, cancellationToken);

        if (updateResult.IsFailure)
        {
            return NotFound(updateResult.Error);
        }

        return Ok(updateResult.Value);
    }


    [Authorize]
    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> DeleteProduct(
        [FromServices] ICommandHandler<DeleteProductDto, DeleteProductQuery> handler,
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        var query = new DeleteProductQuery(new DeleteProductDto(productId));

        var deleteResult = await handler.Handle(query, cancellationToken);

        if (deleteResult.IsFailure)
        {
            return NotFound(deleteResult.Error);
        }

        return Ok(productId);
    }
}