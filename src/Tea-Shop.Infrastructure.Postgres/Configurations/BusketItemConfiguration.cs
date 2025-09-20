using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain.Buskets;
using Tea_Shop.Domain.Products;

namespace Tea_Shop.Infrastructure.Postgres.Configurations;

public class BusketItemConfiguration: IEntityTypeConfiguration<BusketItem>
{
    public void Configure(EntityTypeBuilder<BusketItem> builder)
    {
        builder.ToTable("buskets_items");

        builder
            .HasKey(bi => bi.Id)
            .HasName("pk_buskets_items");

        builder
            .Property(bi => bi.Id)
            .HasConversion(bii => bii.Value, id => new BusketItemId(id))
            .HasColumnName("id");

        builder
            .Property(bi => bi.BusketId)
            .HasConversion(bii => bii.Value, id => new BusketId(id))
            .HasColumnName("busket_id");

        builder
            .Property(bi => bi.ProductId)
            .HasConversion(pi => pi.Value, id => new ProductId(id))
            .HasColumnName("product_id");

        builder
            .Property(bi => bi.Quantity)
            .HasColumnName("quantity");

        builder
            .HasOne<Product>()
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Busket>()
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}