using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Abstractions;
using Tea_Shop.Application.Users.Commands.CreateUserCommand;
using Tea_Shop.Application.Users.Commands.DeleteUserCommand;
using Tea_Shop.Application.Users.Commands.UpdateUserCommand;
using Tea_Shop.Application.Users.Queries;
using Tea_Shop.Contract;
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
                request.AvatarId,
                request.MiddleName,
                fileDto));

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