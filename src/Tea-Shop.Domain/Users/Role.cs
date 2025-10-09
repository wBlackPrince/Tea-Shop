namespace Tea_Shop.Domain.Users;


public sealed class Role
{
    public const int UserRoleId = 1;
    public const int AdminRoleId = 2;

    public const string UserRoleName = "USER";
    public const string AdminRoleName = "ADMIN";

    /// <summary>
    /// Gets or sets Идентифкатор роли.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets Имя роли..
    /// </summary>
    public string Name { get; set; }

    public static Role CreateUserRole()
    {
        return new Role
        {
            Id = Role.UserRoleId,
            Name = Role.UserRoleName,
        };
    }

    public static Role CreateAdminRole()
    {
        return new Role
        {
            Id = Role.AdminRoleId,
            Name = Role.AdminRoleName,
        };
    }
}