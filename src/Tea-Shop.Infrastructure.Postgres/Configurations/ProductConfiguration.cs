using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain.Products;
using Tea_Shop.Shared;

namespace Tea_Shop.Infrastructure.Postgres.Configurations;

public class ProductConfiguration: IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder
            .HasKey(p => p.Id)
            .HasName("pk_products");

        builder.Property(p => p.Id)
            .HasConversion(p => p.Value, id => new ProductId(id));

        builder
            .Property(p => p.Title)
            .HasColumnName("title")
            .HasMaxLength(Constants.Limit100);

        builder
            .Property(p => p.Description)
            .HasColumnName("description")
            .HasMaxLength(Constants.Limit1000);

        builder.Property(p => p.Season)
            .HasConversion(
                p => p.ToString(),
                season => (Season)Enum.Parse(typeof(Season), season))
            .HasColumnName("season");

        builder.Property(p => p.Price)
            .HasColumnName("price");

        builder.Property(p => p.Amount)
            .HasColumnName("amount");

        builder.Property(p => p.Rating)
            .HasColumnName("rating");

        builder.OwnsOne(p => p.PreparationMethod, pb =>
        {
            pb.Property(pm => pm.PreparationTime)
                .IsRequired()
                .HasColumnName("preparation_time");
            pb.Property(pm => pm.Description)
                .IsRequired()
                .HasMaxLength(Constants.Limit100)
                .HasColumnName("preparation_description");
        });

        builder.Navigation(p => p.PreparationMethod).IsRequired(false);

        builder.OwnsMany(
            p => p.Ingrindients,
            pb =>
            {
                pb.ToJson("ingredients");

                pb.Property(i => i.Name)
                    .HasMaxLength(Constants.Limit50)
                    .HasColumnName("ingredient_name");

                pb.Property(i => i.Amount)
                    .HasColumnName("ingredient_amount");

                pb.Property(i => i.IsAllergen)
                    .HasColumnName("ingredient_is_allergen");
            });
    }
}