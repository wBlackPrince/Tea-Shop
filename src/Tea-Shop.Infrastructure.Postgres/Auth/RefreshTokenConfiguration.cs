using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain;
using Tea_Shop.Domain.Tokens;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.Auth;

internal sealed class RefreshTokenConfiguration: IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder
            .HasKey(r => r.Id)
            .HasName("refresh_tokens_pk");

        builder
            .Property(r => r.Token)
            .HasMaxLength(200)
            .IsRequired()
            .HasColumnName("token");

        builder
            .Property(r => r.UserId)
            .HasConversion(u => u.Value, id => new UserId(id))
            .HasColumnName("user_id");

        builder
            .Property(r => r.ExpireOnUtc)
            .HasColumnName("expire_on_utc");

        builder
            .HasIndex(r => r.Token)
            .IsUnique();

        builder
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);
    }
}