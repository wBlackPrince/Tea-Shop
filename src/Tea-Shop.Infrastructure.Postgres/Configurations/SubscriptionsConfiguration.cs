using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain.Subscriptions;
using Tea_Shop.Domain.Users;

namespace Tea_Shop.Infrastructure.Postgres.Configurations;

public sealed class SubscriptionsConfiguration: IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("subscriptions");

        builder
            .HasKey(s => s.Id)
            .HasName("ipk_subscriptions");

        builder
            .Property(s => s.Id)
            .HasConversion(s => s.Value, id => new SubscriptionId(id))
            .HasColumnName("id");

        builder.OwnsOne(s => s.State, sb =>
        {
            sb
                .Property(b => b.Interval)
                .HasColumnName("interval");

            sb
                .Property(b => b.IntervalType)
                .HasConversion(
                    p => p.ToString(),
                    season => (IntervalType)Enum.Parse(typeof(IntervalType), season))
                .HasColumnName("interval_type");

            sb
                .Property(b => b.IsActive)
                .HasColumnName("is_active");

            sb
                .Property(b => b.StatusDuration)
                .HasColumnName("status_duration");
        });

        builder
            .Property(s => s.UserId)
            .HasConversion(u => u.Value, id => new UserId(id))
            .HasColumnName("user_id");

        builder
            .Property(s => s.KitId)
            .HasConversion(u => u.Value, id => new KitId(id))
            .HasColumnName("kit_id");

        builder
            .Property(s => s.CreatedAt)
            .HasColumnName("created_at");

        builder
            .Property(s => s.UpdatedAt)
            .HasColumnName("updated_at");

        builder
            .Property(s => s.LastOrder)
            .HasColumnName("last_order");

        builder
            .HasOne(u => u.User)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Kit>(s => s.Kit)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}