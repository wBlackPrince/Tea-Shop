using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.ValueObjects;
using Subscriptions.Domain;

namespace Subscriptions.Infrastructure.Postgres;

public class SubscriptionsConfiguration: IEntityTypeConfiguration<Subscription>
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
                str => SubscriptionState.ConverToState(str));

        builder
            .Property(s => s.UserId)
            .HasConversion(u => u.Value, id => new UserId(id))
            .HasColumnName("user_id");

        builder
            .HasOne<object>()
            .WithMany()
            .HasForeignKey(k => k.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Kit>(s => s.Kit)
            .WithMany()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}