using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.CommentId);

            builder.Property(c => c.CommentId)
              .IsRequired();

            builder.Property(c => c.Body)
              .IsRequired();

            builder.Property(c => c.CreatedAt)
              .IsRequired();

            builder.Property(c => c.AuthorId)
              .IsRequired();

            builder.Property(c => c.PhotoId)
              .IsRequired();

            builder.HasOne(c => c.Author)
              .WithMany(a => a.Comments)
              .HasForeignKey(c => c.AuthorId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Photo)
              .WithMany(p => p.Comments)
              .HasForeignKey(c => c.PhotoId)
              .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
