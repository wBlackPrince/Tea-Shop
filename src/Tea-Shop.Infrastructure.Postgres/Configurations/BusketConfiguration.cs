using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain.Buskets;
using Tea_Shop.Domain.Orders;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.Configurations;

public class BusketConfiguration: IEntityTypeConfiguration<Busket>
{
    public void Configure(EntityTypeBuilder<Busket> builder)
    {
        builder.ToTable("buskets");

        builder
            .HasKey(b => b.Id)
            .HasName("pk_buskets");

        builder
            .Property(b => b.Id)
            .HasConversion(bi => bi.Value, id => new BusketId(id))
            .HasColumnName("id");

        builder
            .Property(b => b.UserId)
            .HasConversion(ui => ui.Value, id => new UserId(id))
            .HasColumnName("user_id");
    }
}