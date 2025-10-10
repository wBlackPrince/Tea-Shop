using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Users.Commands.AddBasketItemCommand;
using Tea_Shop.Application.Users.Commands.CreateUserCommand;
using Tea_Shop.Application.Users.Commands.DeleteUserCommand;
using Tea_Shop.Application.Users.Commands.LoginUserCommand;
using Tea_Shop.Application.Users.Commands.LoginUserWithRefreshTokenCommand;
using Tea_Shop.Application.Users.Commands.RemoveBasketItemCommand;
using Tea_Shop.Application.Users.Commands.RevokeRefreshTokensCommand;
using Tea_Shop.Application.Users.Commands.UpdateUserCommand;
using Tea_Shop.Application.Users.Commands.VerifyEmailCommand;
using Tea_Shop.Application.Users.Queries.GetBasketByIdQuery;
using Tea_Shop.Application.Users.Queries.GetBasketItemByIdQuery;
using Tea_Shop.Application.Users.Queries.GetUserByIdQuery;
using Tea_Shop.Application.Users.Queries.GetUserCommentsQuery;
using Tea_Shop.Application.Users.Queries.GetUserOrdersQuery;
using Tea_Shop.Application.Users.Queries.GetUserReviewsQuery;
using Tea_Shop.Application.Users.Queries.GetUsersQuery;
using Tea_Shop.Contract;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Users;

[ApiController]
[Route("[controller]")]
public class UsersController: ControllerBase
{
    [Authorize(Roles = Role.AdminRoleName)]
    [Authorize(Roles = Role.UserRoleName)]
    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<GetUserResponseDto>> GetUserById(
        [FromRoute] Guid userId,
        [FromServices] IQueryHandler<GetUserResponseDto, GetUserByIdQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(new UserWithOnlyIdDto(userId));

        var user = await handler.Handle(
            query,
            cancellationToken);

        return Ok(user);
    }

    [Authorize(Roles = Role.AdminRoleName)]
    [HttpGet]
    public async Task<IActionResult> GetUsers(
        [FromQuery] GetUsersRequestDto request,
        [FromServices] IQueryHandler<GetUsersResponseDto, GetUsersQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery(request);

        var users = await handler.Handle(query, cancellationToken);

        return Ok(users);
    }

    [Authorize(Roles = Role.AdminRoleName)]
    [Authorize(Roles = Role.UserRoleName)]
    [HttpGet("orders")]
    public async Task<ActionResult<GetUserOrdersResponseDto?>> GetUserOrders(
        [FromQuery] GetUserOrdersRequestDto request,
        [FromServices] IQueryHandler<GetUserOrdersResponseDto?, GetUserOrdersQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetUserOrdersQuery(request);

        var userOrders = await handler.Handle(query, cancellationToken);

        return Ok(userOrders);
    }

    [Authorize(Roles = Role.AdminRoleName)]
    [Authorize(Roles = Role.UserRoleName)]
    [HttpGet("comments")]
    public async Task<ActionResult<GetUserCommentsResponseDto?>> GetUserComments(
        [FromQuery] GetUserWithPaginationRequestDto request,
        [FromServices] IQueryHandler<GetUserCommentsResponseDto?, GetUserCommentsQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetUserCommentsQuery(request);

        var userComments = await handler.Handle(query, cancellationToken);

        return Ok(userComments);
    }

    [Authorize(Roles = Role.AdminRoleName)]
    [Authorize(Roles = Role.UserRoleName)]
    [HttpGet("reviews")]
    public async Task<ActionResult<GetUserReviewsResponseDto?>> GetUserReviews(
        [FromQuery] GetUserWithPaginationRequestDto request,
        [FromServices] IQueryHandler<GetUserReviewsResponseDto?, GetUserReviewsQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetUserReviewsQuery(request);

        var userReviews = await handler.Handle(query, cancellationToken);

        return Ok(userReviews);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(10_000_000)]
    public async Task<ActionResult<CreateUserResponseDto>> CreateUser(
        [FromForm] CreateUserHttpRequestDto request,
        [FromServices] ICommandHandler<CreateUserResponseDto, CreateUserCommand> handler,
        CancellationToken cancellationToken)
    {
        UploadFileDto? fileDto = null;

        if (request.AvatarFile is not null && request.AvatarFile.Length > 0)
        {
            fileDto = new UploadFileDto(
                Stream: request.AvatarFile.OpenReadStream(),
                FileName: request.AvatarFile.FileName,
                ContentType: request.AvatarFile.ContentType);
        }

        var command = new CreateUserCommand(
            new CreateUserRequestDto(
                request.Password,
                request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber,
                request.Role,
                request.MiddleName,
                fileDto));

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> LoginUser(
        [FromBody] LoginRequestDto request,
        [FromServices] ICommandHandler<LoginResponseDto, LoginUserCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost("login-with-refresh-token")]
    public async Task<ActionResult<LoginResponseDto>> LoginWithRefreshToken(
        [FromBody] LoginWithRefreshTokenRequestDto request,
        [FromServices] ICommandHandler<LoginResponseDto, LoginUserWithRefreshTokenCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserWithRefreshTokenCommand(request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("verify-email", Name = Constants.VerifyEmail)]
    public async Task<IActionResult> VerifyUserEmail(
        [FromQuery] Guid token,
        [FromServices] VerifyEmail handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(token, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [HttpDelete("{userId:guid}/revoke-refresh-tokens")]
    public async Task<ActionResult<bool>> RevokeRefreshToken(
        [FromRoute] Guid userId,
        [FromServices] RevokeRefreshTokensHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(userId, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }



    [Authorize(Roles = Role.AdminRoleName)]
    [Authorize(Roles = Role.UserRoleName)]
    [HttpPatch("{userId:guid}")]
    public async Task<IActionResult> UpdateUser(
        [FromRoute] Guid userId,
        [FromBody] JsonPatchDocument<User> userUpdates,
        [FromServices] UpdateUserHandler handler,
        CancellationToken cancellationToken)
    {
        var updateResult = await handler.Handle(userId, userUpdates, cancellationToken);

        if (updateResult.IsFailure)
        {
            return NotFound(updateResult.Error);
        }

        return Ok(updateResult.Value);
    }

    [Authorize(Roles = Role.AdminRoleName)]
    [Authorize(Roles = Role.UserRoleName)]
    [HttpDelete("{userId:guid}")]
    public async Task<ActionResult<UserWithOnlyIdDto>> DeleteUser(
        [FromRoute] Guid userId,
        [FromServices] ICommandHandler<UserWithOnlyIdDto, DeleteUserCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(new UserWithOnlyIdDto(userId));

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }




    [Authorize(Roles = Role.AdminRoleName)]
    [Authorize(Roles = Role.UserRoleName)]
    [HttpGet("baskets/{basketId:guid}")]
    public async Task<ActionResult<BasketDto?>> GetBasketById(
        [FromRoute] Guid basketId,
        [FromServices] IQueryHandler<BasketDto?, GetBasketByIdQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetBasketByIdQuery(new BasketId(basketId));

        var basket = await handler.Handle(
            query,
            cancellationToken);

        return Ok(basket);
    }

    [Authorize(Roles = Role.AdminRoleName)]
    [Authorize(Roles = Role.UserRoleName)]
    [HttpGet("basket-items/{basketItemId:guid}")]
    public async Task<ActionResult<BasketDto?>> GetBasketItemById(
        [FromRoute] Guid basketId,
        [FromServices] IQueryHandler<BasketItemDto?, GetBasketItemByIdQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetBasketItemByIdQuery(new BasketItemId(basketId));

        var basket = await handler.Handle(
            query,
            cancellationToken);

        return Ok(basket);
    }

    [Authorize(Roles = Role.AdminRoleName)]
    [Authorize(Roles = Role.UserRoleName)]
    [HttpPost("basket-items")]
    public async Task<ActionResult<CreateOrderResponseDto>> AddBasketItem(
        [FromBody] AddBasketItemDto request,
        [FromServices] ICommandHandler<AddBasketItemDto?, AddBasketItemCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new AddBasketItemCommand(request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [Authorize(Roles = Role.AdminRoleName)]
    [Authorize(Roles = Role.UserRoleName)]
    [Authorize]
    [HttpDelete("basket-items")]
    public async Task<ActionResult<CreateOrderResponseDto>> RemoveBasketItem(
        [FromBody] RemoveBasketItemDto request,
        [FromServices] ICommandHandler<RemoveBasketItemDto?, RemoveBasketItemCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new RemoveBasketItemCommand(request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}