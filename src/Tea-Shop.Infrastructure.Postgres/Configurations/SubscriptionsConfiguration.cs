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

        builder.Property(s => s.State)
            .HasConversion(
                s => SubscriptionState.ConverToString(s),
                str => SubscriptionState.ConverToState(str))
            .HasColumnName("state");

        builder
            .Property(s => s.UserId)
            .HasConversion(u => u.Value, id => new UserId(id))
            .HasColumnName("user_id");

        builder
            .Property(s => s.KitId)
            .HasConversion(u => u.Value, id => new KitId(id))
            .HasColumnName("kit_id");

        builder
            .HasOne<User>()
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