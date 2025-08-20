using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Domain.Products;
using Tea_Shop.Domain.Reviews;

namespace Tea_Shop.Infrastructure.Postgres.Configurations;

public class ReviewConfiguration: IEntityTypeConfiguration<Review>
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
            .Property(r => r.Title)
            .HasColumnName("title");

        builder
            .Property(r => r.Text)
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
            .HasColumnName("product_id");

        builder
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(p => p.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany<Comment>()
            .WithOne()
            .HasForeignKey(c => c.ReviewId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}