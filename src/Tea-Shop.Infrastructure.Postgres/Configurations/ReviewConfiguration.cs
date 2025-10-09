using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Infrastructure.Postgres.Configurations;

public sealed class ReviewConfiguration: IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");

        builder
            .HasKey(r => r.Id)
            .HasName("pk_reviews");

        builder
            .Property(r => r.Id)
            .HasConversion(r => r.Value, id => new ReviewId(id))
            .HasColumnName("id");

        builder
            .Property(r => r.ProductRating)
            .HasColumnName("product_rating");

        builder
            .Property(r => r.Title)
            .HasMaxLength(Constants.Limit50)
            .HasColumnName("title");

        builder
            .Property(r => r.Text)
            .HasMaxLength(Constants.Limit2000)
            .HasColumnName("text");

        builder
            .Property(r => r.Rating)
            .HasColumnName("rating");

        builder
            .Property(r => r.CreatedAt)
            .HasColumnName("created_at");

        builder
            .Property(r => r.UpdatedAt)
            .HasColumnName("updated_at");

        builder
            .Property(r => r.ProductId)
            .HasConversion(r => r.Value, id => new ProductId(id))
            .HasColumnName("product_id");

        builder
            .Property(r => r.UserId)
            .HasConversion(r => r.Value, id => new UserId(id))
            .HasColumnName("user_id");

        builder
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(p => p.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}