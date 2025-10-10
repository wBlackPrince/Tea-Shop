using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder
            .HasKey(u => u.Id)
            .HasName("pk_users");

        builder
            .Property(u => u.Id)
            .HasConversion(u => u.Value, id => new UserId(id))
            .HasColumnName("id");

        builder
            .Property(u => u.BasketId)
            .HasConversion(bi => bi.Value, id => new BasketId(id))
            .HasColumnName("basket_id");

        builder.Property(u => u.Password)
            .HasColumnName("password");

        builder.Property(u => u.FirstName)
            .HasColumnName("first_name");

        builder.Property(u => u.LastName)
            .HasColumnName("last_name");

        builder.Property(u => u.MiddleName)
            .HasColumnName("middle_name");

        builder.Property(u => u.Email)
            .HasColumnName("email");

        builder.Property(u => u.EmailVerified)
            .HasColumnName("email_verified");

        builder.Property(u => u.BonusPoints)
            .HasColumnName("bonus_points");

        builder.Property(u => u.PhoneNumber)
            .HasColumnName("phone_number");

        builder.Property(u => u.AvatarId)
            .HasColumnName("avatar_id");

        builder.Property(u => u.IsActive)
            .HasColumnName("is_active");

        builder
            .HasMany<Review>()
            .WithOne()
            .HasForeignKey(r => r.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany<Comment>()
            .WithOne()
            .HasForeignKey(c => c.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Basket>()
            .WithOne()
            .HasForeignKey<Basket>(r => r.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}