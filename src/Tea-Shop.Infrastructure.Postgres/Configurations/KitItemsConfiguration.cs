using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Subscriptions;

namespace Tea_Shop.Infrastructure.Postgres.Configurations;

public class KitItemsConfiguration: IEntityTypeConfiguration<KitItem>
{
    public void Configure(EntityTypeBuilder<KitItem> builder)
    {
        builder.ToTable("kit_items");

        builder
            .HasKey(ki => ki.Id)
            .HasName("kit_items_id");

        builder
            .Property(ki => ki.Id)
            .HasConversion(ki => ki.Value, id => new KitItemId(id))
            .HasColumnName("id");

        builder
            .Property(ki => ki.ProductId)
            .HasConversion(ki => ki.Value, id => new ProductId(id))
            .HasColumnName("product_id");

        builder
            .Property(ki => ki.Amount)
            .HasColumnName("amount");

        builder
            .Property(ki => ki.KitId)
            .HasConversion(ki => ki.Value, id => new KitId(id))
            .HasColumnName("kit_id");

        builder
            .HasOne<Product>()
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}