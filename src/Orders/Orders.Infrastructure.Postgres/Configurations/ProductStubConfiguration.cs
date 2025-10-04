using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain;
using Shared.ValueObjects;

namespace Orders.Infrastructure.Postgres.Configurations;

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