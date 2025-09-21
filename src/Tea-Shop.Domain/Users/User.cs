using System.ComponentModel.DataAnnotations;
using Tea_Shop.Domain.Baskets;

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
    /// <param name="avatarId">Идентификатор аватара.</param>
    /// <param name="middleName">Отчество.</param>
    public User(
        UserId id,
        string password,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        Role role,
        Guid? avatarId = null,
        string middleName = "")
    {
        Id = id;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        EmailVerified = false;
        PhoneNumber = phoneNumber;
        Role = role;
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
    public Role Role { get; set; }

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