using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Infrastructure.Postgres.Configurations;

public class RoleConfiguration: IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder
            .HasKey(r => r.Id)
            .HasName("ipk_roles");

        builder
            .Property(r => r.Id)
            .HasColumnName("id");

        builder
            .Property(r => r.Name)
            .HasMaxLength(Constants.Limit50)
            .HasColumnName("name")
            .IsRequired();

        builder.HasData(
            new Role { Id = Role.UserRoleId, Name = Role.UserRoleName },
            new Role { Id = Role.AdminRoleId, Name = Role.AdminRoleName });
    }
}