using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.ValueObjects;
using Users.Domain;

namespace Users.Infrastructure.Postgres.Configurations;

public class BasketItemConfiguration: IEntityTypeConfiguration<BasketItem>
{
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        builder.ToTable("baskets_items");

        builder
            .HasKey(bi => bi.Id)
            .HasName("pk_baskets_items");

        builder
            .Property(bi => bi.Id)
            .HasConversion(bii => bii.Value, id => new BasketItemId(id))
            .HasColumnName("id");

        builder
            .Property(bi => bi.BasketId)
            .HasConversion(bii => bii.Value, id => new BasketId(id))
            .HasColumnName("basket_id");

        builder
            .Property(bi => bi.ProductId)
            .HasConversion(pi => pi.Value, id => new ProductId(id))
            .HasColumnName("product_id");

        builder
            .Property(bi => bi.Quantity)
            .HasColumnName("quantity");

        builder
            .HasOne<ProductStub>()
            .WithMany()
            .HasForeignKey(bi => bi.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}