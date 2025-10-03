using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Domain;
using Shared;
using Shared.ValueObjects;

namespace Products.Infrastructure.Postgres.Configurations;

public class ProductConfiguration: IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");

        builder
            .HasKey(p => p.Id)
            .HasName("pk_products");

        builder
            .Property(p => p.Id)
            .HasConversion(p => p.Value, id => new ProductId(id))
            .HasColumnName("id");

        builder
            .Property(p => p.Title)
            .HasColumnName("title")
            .HasMaxLength(Constants.Limit100);

        builder
            .Property(p => p.Description)
            .HasColumnName("description")
            .HasMaxLength(Constants.Limit2000);

        builder.Property(p => p.Season)
            .HasConversion(
                p => p.ToString(),
                season => (Season)Enum.Parse(typeof(Season), season))
            .HasColumnName("season");

        builder.Property(p => p.Price)
            .HasColumnName("price");

        builder.Property(p => p.Amount)
            .HasColumnName("amount");

        builder.Property(p => p.StockQuantity)
            .HasColumnName("stock_quantity");

        builder.Property(p => p.SumRatings)
            .HasColumnName("sum_ratings");

        builder.Property(p => p.CountRatings)
            .HasColumnName("count_ratings");

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(p => p.UpdatedAt)
            .HasColumnName("updated_at");

        builder.OwnsOne(
            p => p.PreparationMethod,
            pb =>
            {
                pb.ToJson("ingredients");

                pb.Property(p => p.PreparationTime)
                    .HasColumnName("preparation_time");

                pb.Property(p => p.Description)
                    .HasColumnName("preparation_description");

                pb.OwnsMany(p => p.Ingredients, ib =>
                {
                    ib.Property(i => i.Name)
                        .HasMaxLength(Constants.Limit50)
                        .HasColumnName("ingredient_name");

                    ib.Property(i => i.Amount)
                        .HasColumnName("ingredient_amount");

                    ib.Property(i => i.Description)
                        .HasMaxLength(Constants.Limit2000)
                        .HasColumnName("ingredient_description");

                    ib.Property(i => i.IsAllergen)
                        .HasColumnName("ingredient_is_allergen");
                });

            });

        builder.Property(p => p.PhotosIds)
            .HasColumnName("photos_ids");
    }
}