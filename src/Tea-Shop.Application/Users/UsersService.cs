using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Tea_Shop.Contract.Users;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.Users;

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


    public async Task<Result<GetUserResponseDto, Error>> GetById(Guid userId, CancellationToken cancellationToken)
    {
        var getResult = await _usersRepository.GetUser(new UserId(userId), cancellationToken);

        if (getResult.IsFailure)
        {
            return getResult.Error;
        }

        User user = getResult.Value;

        var response = new GetUserResponseDto(
            user.Password,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.Role.ToString(),
            user.AvatarId,
            user.MiddleName);

        return response;
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

    public async Task<Result<Guid, Error>> UpdateUser(
        Guid userId,
        JsonPatchDocument<User> userUpdates,
        CancellationToken cancellationToken)
    {
        var (_, isFailure, user, error) = await _usersRepository.GetUser(
            new UserId(userId),
            cancellationToken);

        if (isFailure)
        {
            return error;
        }

        userUpdates.ApplyTo(user);
        await _usersRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Update user with id {UserId}", userId);

        return user.Id.Value;
    }

    public async Task<Result<Guid, Error>> DeleteUser(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var deleteResult = await _usersRepository.DeleteUser(new UserId(userId), cancellationToken);

        if (deleteResult.IsFailure)
        {
            return deleteResult.Error;
        }

        _logger.LogInformation("User with id {UserId} deleted.", userId);

        return userId;
    }
}