using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain.Products;

namespace Tea_Shop.Infrastructure.Postgres.Configurations;

public class ProductsTagsConfiguration: IEntityTypeConfiguration<ProductsTags>
{
    public void Configure(EntityTypeBuilder<ProductsTags> builder)
    {
        builder.ToTable("products_tags");

        builder.HasKey(p => p.Id)
            .HasName("pk_products_tags_id");

        builder.Property(p => p.Id)
            .HasConversion(
                p => p.Value,
                id => new ProductsTagsId(id));
    }
}