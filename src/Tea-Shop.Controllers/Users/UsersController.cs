using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Users.Commands.CreateUserCommand;
using Tea_Shop.Application.Users.Commands.DeleteUserCommand;
using Tea_Shop.Application.Users.Commands.LoginUserCommand;
using Tea_Shop.Application.Users.Commands.UpdateUserCommand;
using Tea_Shop.Application.Users.Queries.GetUserByIdQuery;
using Tea_Shop.Application.Users.Queries.GetUserCommentsQuery;
using Tea_Shop.Application.Users.Queries.GetUserOrdersQuery;
using Tea_Shop.Application.Users.Queries.GetUserReviewsQuery;
using Tea_Shop.Application.Users.Queries.GetUsersQuery;
using Tea_Shop.Contract;
using Tea_Shop.Contract.Orders;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Users;

[ApiController]
[Route("[controller]")]
public class UsersController: ControllerBase
{
    [Authorize]
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

    [Authorize]
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(
        [FromQuery] GetUsersRequestDto request,
        [FromServices] IQueryHandler<GetUsersResponseDto, GetUsersQuery> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery(request);

        var users = await handler.Handle(query, cancellationToken);

        return Ok(users);
    }

    [Authorize]
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

    [Authorize]
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

    [Authorize]
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

    [Authorize]
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
    public async Task<ActionResult<string>> LoginUser(
        [FromBody] LoginRequestDto request,
        [FromServices] ICommandHandler<string, LoginUserCommand> handler,
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

    [Authorize]
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

    [Authorize]
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
}