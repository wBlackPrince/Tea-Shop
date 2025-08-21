using Microsoft.AspNetCore.Mvc;
using Tea_Shop.Contract.Users;
using Tea_Shop.Infrastructure.Postgres.Repositories;

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

    [HttpPost]
    public async Task<IActionResult> CreateUser(
        [FromBody]CreateUserRequestDto request,
        CancellationToken cancellationToken)
    {
        await _usersService.CreateUser(request, cancellationToken);

        return Ok();
    }
}