using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain.Baskets;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.Configurations;

public sealed class BasketConfiguration: IEntityTypeConfiguration<Basket>
{
    public void Configure(EntityTypeBuilder<Basket> builder)
    {
        builder.ToTable("baskets");

        builder
            .HasKey(b => b.Id)
            .HasName("pk_baskets");

        builder
            .Property(b => b.Id)
            .HasConversion(bi => bi.Value, id => new BasketId(id))
            .HasColumnName("id");

        builder
            .Property(b => b.UserId)
            .HasConversion(ui => ui.Value, id => new UserId(id))
            .HasColumnName("user_id");

        builder.HasMany(b => b.Items)
            .WithOne()
            .HasForeignKey(i => i.BasketId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}