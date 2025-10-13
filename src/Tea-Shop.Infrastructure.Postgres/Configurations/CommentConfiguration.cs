using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tea_Shop.Domain.Social;
using Tea_Shop.Domain.Users;
using Tea_Shop.Shared;
using Path = Tea_Shop.Domain.Social.Path;

namespace Tea_Shop.Infrastructure.Postgres.Configurations;

public sealed class CommentConfiguration: IEntityTypeConfiguration<Comment>
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
            .IsRequired(false)
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

        builder
            .Property(c => c.Identifier)
            .IsRequired()
            .HasConversion(ident => ident.Value, id => new Identifier(id))
            .HasColumnName("identifier");

        builder
            .Property(c => c.Path)
            .HasConversion(c => c.Value, path => Path.Create(path))
            .HasColumnName("path")
            .HasColumnType("ltree");

        builder
            .HasIndex(c => c.Path)
            .HasMethod("gist")
            .HasDatabaseName("idx_departments_path");

        builder
            .Property(c => c.Depth)
            .HasColumnName("depth");

        builder
            .HasMany<Comment>()
            .WithOne(c => c.ParentComment)
            .IsRequired(false)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}