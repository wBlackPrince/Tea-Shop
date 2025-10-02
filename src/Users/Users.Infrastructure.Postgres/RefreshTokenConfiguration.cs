using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Application;

namespace Users.Infrastructure.Postgres;

internal sealed class RefreshTokenConfiguration: IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");

        builder
            .HasKey(r => r.Id)
            .HasName("refresh_tokens_pk");

        builder.Property(r => r.Token).HasMaxLength(200).IsRequired();

        builder.HasIndex(r => r.Token).IsUnique();

        builder
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId);
    }
}