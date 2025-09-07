using Microsoft.AspNetCore.Http;

namespace Tea_Shop.Users;

public record CreateUserHttpRequestDto
{
    public string Password { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Email { get; init; }

    public string PhoneNumber { get; init; }

    public string Role { get; init; }

    public Guid? AvatarId { get; init; }

    public string MiddleName { get; init; }

    public IFormFile? AvatarFile { get; init; }
};