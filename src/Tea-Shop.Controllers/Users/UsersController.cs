using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Application.Users;
using Tea_Shop.Contract.Users;
using Tea_Shop.Shared;

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
    public async Task<IActionResult> GetUsers(
        [FromRoute]Guid userId,
        CancellationToken cancellationToken)
    {
        var getResult = await _usersService.Get(userId, cancellationToken);

        if (getResult.IsFailure)
        {
            return BadRequest(getResult.Error);
        }

        return Ok(getResult.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(
        [FromBody]CreateUserRequestDto request,
        CancellationToken cancellationToken)
    {
        await _usersService.CreateUser(request, cancellationToken);

        return Ok();
    }
}