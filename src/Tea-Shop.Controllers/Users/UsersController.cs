using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Users;
using Tea_Shop.Application.Users.Commands.CreateUserCommand;
using Tea_Shop.Application.Users.Commands.DeleteUserCommand;
using Tea_Shop.Application.Users.Commands.UpdateUserCommand;
using Tea_Shop.Application.Users.Queries;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Users;

[ApiController]
[Route("[controller]")]
public class UsersController: ControllerBase
{
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUserById(
        [FromRoute] Guid userId,
        [FromServices] GetUserByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(userId, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveUsers(
        [FromServices] GetActiveUsersHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("banned")]
    public async Task<IActionResult> GetBannedUsers(
        [FromServices] GetBannedUsersHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserResponseDto>> CreateUser(
        [FromBody] CreateUserRequestDto request,
        [FromServices] ICommandHandler<CreateUserResponseDto, CreateUserCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(request);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

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

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> DeleteUser(
        [FromRoute] Guid userId,
        [FromServices] DeleteUserHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(userId, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(userId);
    }
}