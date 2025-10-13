namespace Tea_Shop.Contract.Users;

public record CreateUserResponseDto
{
    public Guid Id { get; init; }

    public string Password { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Address { get; init; }

    public string Email { get; init; }

    public string PhoneNumber { get; init; }

    public string Role { get; init; }

    public Guid? AvatarId { get; init; }

    public string MiddleName { get; init; }
}