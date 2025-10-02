using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.ValueObjects;
using Users.Application;

namespace Users.Infrastructure.Postgres;

public sealed class EmailVerificationTokenConfiguration: IEntityTypeConfiguration<EmailVerificationToken>
{
    public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
    {
        builder.ToTable("email_verification_tokens");

        builder
            .HasKey(k => k.Id)
            .HasName("email_verification_tokens_pk");

        builder
            .Property(k => k.Id)
            .HasColumnName("id");

        builder
            .Property(k => k.UserId)
            .HasConversion(uid => uid.Value, id => new UserId(id))
            .HasColumnName("user_id");

        builder
            .Property(k => k.CreatedOnUtc)
            .HasColumnName("created_on_utc");

        builder
            .Property(k => k.ExpiresOnUtc)
            .HasColumnName("expires_on_utc");
    }
}