using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain;
using Shared.ValueObjects;

namespace Orders.Infrastructure.Postgres;

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

        builder.Property(o => o.Id)
            .HasConversion(o => o.Value, id => new OrderItemId(id));

        builder.Property(o => o.Quantity)
            .HasColumnName("quantity");

        builder.Property(o => o.OrderId)
            .HasColumnName("order_id");

        builder
            .Property(oi => oi.ProductId)
            .HasColumnName("product_id");
    }
}