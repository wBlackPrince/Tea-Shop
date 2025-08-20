using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Reviews;
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

                    ib.Property(i => i.IsAllergen)
                        .HasColumnName("ingredient_is_allergen");
                });

            });

        builder
            .HasMany<OrderItem>()
            .WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany<Review>()
            .WithOne()
            .HasForeignKey(r => r.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}