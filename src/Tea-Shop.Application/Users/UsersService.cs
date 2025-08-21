using FluentValidation;
using Microsoft.Extensions.Logging;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.Repositories;

public class UsersService : IUsersService
{
    private IUsersRepository _usersRepository;
    private IValidator<CreateUserRequestDto> _validator;
    private ILogger<UsersService> _logger;

    public UsersService(
        IUsersRepository usersRepository,
        IValidator<CreateUserRequestDto> validator,
        ILogger<UsersService> logger)
    {
        _usersRepository = usersRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Guid> CreateUser(
        CreateUserRequestDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        User user = new User(
            new UserId(Guid.NewGuid()),
            request.Password,
            request.FirstName,
            request.LastName,
            request.Email,
            request.PhoneNumber,
            (Role)Enum.Parse(typeof(Role), request.Role),
            request.AvatarId,
            request.MiddleName);

        await _usersRepository.CreateUser(user, cancellationToken);

        await _usersRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User with id {UserId} created a new account.", user.Id.Value);

        return user.Id.Value;
    }
    
}