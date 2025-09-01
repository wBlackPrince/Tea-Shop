using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain.Comments;
using Tea_Shop.Domain.Reviews;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;

namespace Tea_Shop.Infrastructure.Postgres.Configurations;

public class CommentConfiguration: IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comments");

        builder
            .HasKey(c => c.Id)
            .HasName("pk_comments");

        builder
            .Property(c => c.Id)
            .HasConversion(c => c.Value, id => new CommentId(id))
            .HasColumnName("id");

        builder
            .Property(c => c.Text)
            .HasMaxLength(Constants.Limit2000)
            .HasColumnName("text");

        builder
            .Property(c => c.Rating)
            .HasColumnName("rating");

        builder
            .Property(c => c.CreatedAt)
            .HasColumnName("created_at");

        builder
            .Property(c => c.UpdatedAt)
            .HasColumnName("updated_at");

        builder
            .Property(c => c.ParentId)
            .HasConversion(p => p.Value, id => new CommentId(id))
            .HasDefaultValue(null)
            .HasColumnName("parent_id");

        builder
            .Property(c => c.ReviewId)
            .HasConversion(r => r.Value, id => new ReviewId(id))
            .HasColumnName("review_id");

        builder
            .Property(c => c.UserId)
            .HasConversion(u => u.Value, id => new UserId(id))
            .HasColumnName("user_id");
    }
}