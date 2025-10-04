using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.ValueObjects;
using Subscriptions.Domain;

namespace Subscriptions.Infrastructure.Postgres.Configurations;

public class ProductStubConfiguration: IEntityTypeConfiguration<ProductStub>
{
    public void Configure(EntityTypeBuilder<ProductStub> builder)
    {
        builder.ToTable(
            "products",
            "products",
            t => t.ExcludeFromMigrations());

        builder.HasKey(p => p.Id);
        builder
            .Property(p => p.Id)
            .HasConversion(pi => pi.Value, id => new ProductId(id))
            .HasColumnName("id");
    }
}