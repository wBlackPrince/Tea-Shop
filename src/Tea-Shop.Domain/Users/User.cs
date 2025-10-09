using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using Tea_Shop.Domain.Baskets;
using Tea_Shop.Shared;

namespace Tea_Shop.Domain.Users;

/// <summary>
/// Domain-модель пользователя
/// </summary>
public class User: Entity
{
    private string _password;
    private string _firstName;
    private string _lastName;
    private string _email;
    private int _bonusPoints;
    private string _middleName;
    private string _phoneNumber;

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="id"> Идентификатор пользователя.</param>
    /// <param name="password"> Пароль.</param>
    /// <param name="firstName"> Имя.</param>
    /// <param name="lastName">Фамилия.</param>
    /// <param name="email">Электронная почта.</param>
    /// <param name="phoneNumber">Номер телефона.</param>
    /// <param name="role">Роль в системе.</param>
    /// <param name="basketId"> Идентификатор коризны.</param>
    /// <param name="avatarId">Идентификатор аватара.</param>
    /// <param name="middleName">Отчество.</param>
    public User(
        UserId id,
        string password,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string role,
        BasketId basketId,
        Guid? avatarId = null,
        string middleName = "")
    {
        Id = id;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        EmailVerified = false;
        _bonusPoints = 0;
        PhoneNumber = phoneNumber;
        Role = role;
        BasketId = basketId;
        AvatarId = avatarId;
        IsActive = true;
        MiddleName = middleName;
    }

    // для Ef Core
    private User()
    {
    }

    /// <summary>
    /// Gets or sets Идентификатор пользователя
    /// </summary>
    public UserId Id { get; set; }

    /// <summary>
    /// Gets or sets Идентификатор корзины
    /// </summary>
    public BasketId BasketId { get; set; }

    /// <summary>
    /// Gets or sets Пароль пользователя
    /// </summary>
    public string Password
    {
        get => _password;
        set => UpdatePassword(value);
    }

    /// <summary>
    /// Gets or sets Имя пользователя
    /// </summary>
    public string FirstName
    {
        get => _firstName;
        set => UpdateFirstName(value);
    }

    /// <summary>
    /// Gets or sets Фамилия пользователя
    /// </summary>
    public string LastName
    {
        get => _lastName;
        set => UpdateLastName(value);
    }

    /// <summary>
    /// Gets or sets Отчество пользователя
    /// </summary>
    public string MiddleName
    {
        get => _middleName;
        set => UpdateMiddleName(value);
    }

    /// <summary>
    /// Gets or sets Email пользователя
    /// </summary>
    public string Email
    {
        get => _email;
        set => UpdateEmail(value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether подтверждение email пользвоателя
    /// </summary>
    public bool EmailVerified { get; set; }

    /// <summary>
    /// Gets бонусные баллы пользователя
    /// </summary>
    public int BonusPoints => _bonusPoints;

    /// <summary>
    /// Gets or sets Номер телефона пользователя
    /// </summary>
    public string PhoneNumber
    {
        get => _phoneNumber;
        set => UpdatePhoneNumber(value);
    }

    /// <summary>
    /// Gets or sets Аватар пользователя
    /// </summary>
    public Guid? AvatarId { get; set; } = null;

    /// <summary>
    /// Gets or sets Роль пользователя
    /// </summary>
    public string Role { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether Способность пользователя пользоваться приложением
    /// </summary>
    public bool IsActive { get; set; } = true;

    public void UpdatePassword(string password)
    {
        var validateResult = CheckAttributeIsNotEmpty(password);

        if (validateResult.IsFailure)
        {
            throw new ValidationException(validateResult.Error.Message);
        }

        _password = password;
    }

    public void UpdateFirstName(string firstName)
    {
        var validateResult = CheckAttributeIsNotEmpty(firstName);

        if (validateResult.IsFailure)
        {
            throw new ValidationException(validateResult.Error.Message);
        }

        _firstName = firstName;
    }

    public void UpdateLastName(string lastName)
    {
        var validateResult = CheckAttributeIsNotEmpty(lastName);

        if (validateResult.IsFailure)
        {
            throw new ValidationException(validateResult.Error.Message);
        }

        _lastName = lastName;
    }

    public void UpdateMiddleName(string middleName) => _middleName = middleName;

    public void UpdateEmail(string email)
    {
        var validateResult = CheckAttributeIsNotEmpty(email);

        if (validateResult.IsFailure)
        {
            throw new ValidationException(validateResult.Error.Message);
        }

        _email = email;
    }

    public void AddBonusPoints(int points)
    {
        _bonusPoints += points;
    }

    public UnitResult<Error> RemoveBonusPoints(int points)
    {
        if (points > _bonusPoints)
        {
            return Error.Validation(
                "remove.bonus_points",
                "There are too little bonuses to remove");
        }

        _bonusPoints -= points;

        return UnitResult.Success<Error>();
    }

    public void UpdatePhoneNumber(string phoneNumber)
    {
        var validateResult = CheckAttributeIsNotEmpty(phoneNumber);

        if (validateResult.IsFailure)
        {
            throw new ValidationException(validateResult.Error.Message);
        }

        _phoneNumber = phoneNumber;
    }
}