using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Users;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Users;

[ApiController]
[Route("[controller]")]
public class UsersController: ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUserById(
        [FromRoute]Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _usersService.GetById(userId, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveUsers(CancellationToken cancellationToken)
    {
        var result = await _usersService.GetActiveUsers(cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("banned")]
    public async Task<IActionResult> GetBannedUsers(CancellationToken cancellationToken)
    {
        var result = await _usersService.GetBannedUsers(cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(
        [FromBody]CreateUserRequestDto request,
        CancellationToken cancellationToken)
    {
        await _usersService.CreateUser(request, cancellationToken);

        return Ok();
    }

    [HttpPatch("{userId:guid}")]
    public async Task<IActionResult> UpdateUser(
        [FromRoute] Guid userId,
        [FromBody] JsonPatchDocument<User> userUpdates,
        CancellationToken cancellationToken)
    {
        var updateResult = await _usersService.UpdateUser(userId, userUpdates, cancellationToken);

        if (updateResult.IsFailure)
        {
            return NotFound(updateResult.Error);
        }

        return Ok(updateResult.Value);
    }

    [HttpDelete("{userId:guid}")]
    public async Task<IActionResult> DeleteUser(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _usersService.DeleteUser(userId, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error);
        }

        return Ok(userId);
    }
}