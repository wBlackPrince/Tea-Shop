using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Domain;
using Shared.ValueObjects;

namespace Products.Infrastructure.Postgres.Configurations;

public class ProductsTagsConfiguration: IEntityTypeConfiguration<ProductsTags>
{
    public void Configure(EntityTypeBuilder<ProductsTags> builder)
    {
        builder.ToTable("products_tags");

        builder.HasKey(p => p.Id)
            .HasName("pk_products_tags_id");

        builder
            .Property(p => p.TagId)
            .HasConversion(
                p => p.Value,
                id => new TagId(id))
            .HasColumnName("tag_id");

        builder.Property(p => p.Id)
            .HasConversion(
                p => p.Value,
                id => new ProductsTagsId(id))
            .HasColumnName("id");

        builder
            .HasOne(p => p.Product)
            .WithMany(p => p.TagsIds)
            .HasForeignKey("product_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Tag>()
            .WithMany()
            .HasForeignKey(pt => pt.TagId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}