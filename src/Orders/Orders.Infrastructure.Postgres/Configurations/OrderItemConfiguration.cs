using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain;
using Shared.ValueObjects;

namespace Orders.Infrastructure.Postgres.Configurations;

public class OrderItemConfiguration: IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");

        builder.HasKey(o => o.Id)
            .HasName("pk_order_items");

        builder
            .Property(oi => oi.Id)
            .HasConversion(oi => oi.Value, id => new OrderItemId(id))
            .HasColumnName("id");

        builder.Property(o => o.ProductId)
            .HasConversion(o => o.Value, id => new ProductId(id))
            .HasColumnName("product_id");

        builder.Property(o => o.Quantity)
            .HasColumnName("quantity");

        builder.Property(o => o.OrderId)
            .HasConversion(o => o.Value, id => new OrderId(id))
            .HasColumnName("order_id");

        builder
            .Property(oi => oi.ProductId)
            .HasColumnName("product_id");

        builder
            .HasOne<ProductStub>()
            .WithMany()
            .HasForeignKey(oi => oi.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}