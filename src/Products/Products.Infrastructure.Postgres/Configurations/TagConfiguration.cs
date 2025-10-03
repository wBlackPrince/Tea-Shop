using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Domain;
using Shared;
using Shared.ValueObjects;

namespace Products.Infrastructure.Postgres.Configurations;

public class TagConfiguration: IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tags");

        builder
            .HasKey(t => t.Id)
            .HasName("pk_tags");

        builder
            .Property(t => t.Id)
            .HasConversion(t => t.Value, id => new TagId(id))
            .HasColumnName("id");

        builder
            .Property(t => t.Name)
            .HasMaxLength(Constants.Limit50)
            .HasColumnName("name");

        builder
            .Property(t => t.Description)
            .HasMaxLength(Constants.Limit500)
            .HasColumnName("description");
    }
}