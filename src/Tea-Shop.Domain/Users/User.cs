using Tea_Shop.Domain.Products;

namespace Tea_Shop.Domain.Users;

/// <summary>
/// Domain-модель пользователя
/// </summary>
public class User
{
    private List<Order> _orders;

    // для Ef Core
    private User() {}

    /// <summary>
    /// Initializes a new instance of the "User" class.
    /// /// </summary>
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
        Guid id,
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
        PhoneNumber = phoneNumber;
        Role = role;
        AvatarId = avatarId;
        IsActive = true;
        MiddleName = middleName;
    }

    /// <summary>
    /// Gets or sets Идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets Пароль пользователя
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets Имя пользователя
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets Фамилия пользователя
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets Отчество пользователя
    /// </summary>
    public string MiddleName { get; set; }

    /// <summary>
    /// Gets or sets Email пользователя
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets Номер телефона пользователя
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets Аватар пользователя
    /// </summary>
    public Guid? AvatarId { get; set; } = null;

    /// <summary>
    /// Gets or sets Роль пользователя
    /// </summary>
    public Role Role { get; set; }

    /// <summary>
    /// Gets or sets Способность пользователя пользоваться приложением
    /// </summary>
    public bool IsActive { get; set; } = true;


    public IReadOnlyList<Order> Orders => _orders;

    public void AddOrder(Order order)
    {
        _orders.Add(order);
    }
}