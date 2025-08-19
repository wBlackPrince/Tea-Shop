using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain.Products;

namespace Tea_Shop.Infrastructure.Postgres.Configurations;

public class OrderItemConfiguration: IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");

        builder.HasKey(o => o.Id)
            .HasName("pk_order_items");

        builder.Property(o => o.Id)
            .HasConversion(o => o.Value, id => new OrderItemId(id));

        builder.Property(o => o.Quantity)
            .HasColumnName("quantity");
    }
}