namespace Shared.Dto;

public record CreateUserHttpRequestDto
{
    public string Password { get; init; } = string.Empty;

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;

    public string Role { get; init; } = string.Empty;

    public string MiddleName { get; init; } = string.Empty;

    public IFormFile? AvatarFile { get; init; }
};