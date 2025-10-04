using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain;
using Shared.ValueObjects;

namespace Orders.Infrastructure.Postgres.Configurations;

public class UserStubConfiguration: IEntityTypeConfiguration<UserStub>
{
    public void Configure(EntityTypeBuilder<UserStub> builder)
    {
        builder.ToTable(
            "users",
            "users",
            t => t.ExcludeFromMigrations());

        builder.HasKey(p => p.Id);
        builder
            .Property(p => p.Id)
            .HasConversion(pi => pi.Value, id => new UserId(id))
            .HasColumnName("id");
    }
}