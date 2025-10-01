namespace Subscriptions.Infrastructure.Postgres;

public class KitDetailsConfiguration : IEntityTypeConfiguration<KitDetails>
{
    public void Configure(EntityTypeBuilder<KitDetails> builder)
    {
        builder.ToTable("kits_details");

        builder
            .HasKey(kd => kd.Id)
            .HasName("kit_details_pk");

        builder
            .Property(kd => kd.Id)
            .HasConversion(kd => kd.Value, id => new KitDetailsId(id))
            .HasColumnName("id");

        builder
            .Property(kd => kd.Description)
            .HasColumnName("description");

        builder
            .Property(kd => kd.Sum)
            .HasColumnName("sum");
    }
}