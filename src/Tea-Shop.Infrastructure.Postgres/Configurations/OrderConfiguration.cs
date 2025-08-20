using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.Configurations;

public class OrderConfiguration: IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder
            .HasKey(o => o.Id)
            .HasName("ipk_orders");

        builder
            .Property(o => o.Id)
            .HasConversion(o => o.Value, id => new OrderId(id))
            .HasColumnName("id");

        builder.Property(o => o.DeliveryAddress)
            .HasColumnName("delivery_address");

        builder.Property(o => o.PaymentWay)
            .HasConversion(
                o => o.ToString(),
                pay_way => (PaymentWay)Enum.Parse(typeof(PaymentWay), pay_way))
            .HasColumnName("payment_way");

        builder.Property(o => o.OrderStatus)
            .HasConversion(
                o => o.ToString(),
                pay_way => (OrderStatus)Enum.Parse(typeof(OrderStatus), pay_way))
            .HasColumnName("order_status");

        builder
            .HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}