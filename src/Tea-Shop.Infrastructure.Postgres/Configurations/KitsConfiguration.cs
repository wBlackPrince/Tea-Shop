using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain.Subscriptions;

namespace Tea_Shop.Infrastructure.Postgres.Configurations;

public sealed class KitsConfiguration: IEntityTypeConfiguration<Kit>
{
    public void Configure(EntityTypeBuilder<Kit> builder)
    {
        builder.ToTable("kits");

        builder.HasKey(k => k.Id).HasName("ipk_kits");

        builder
            .Property(k => k.Id)
            .HasConversion(k => k.Value, id => new KitId(id))
            .HasColumnName("id");

        builder
            .Property(k => k.Name)
            .HasColumnName("name");

        builder
            .Property(k => k.AvatarId)
            .HasColumnName("avatar_id");

        builder
            .HasMany(k => k.KitItems)
            .WithOne()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(k => k.Details)
            .WithOne()
            .HasForeignKey<KitDetails>()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}