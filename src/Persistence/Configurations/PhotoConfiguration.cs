using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
    {
        public void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .IsRequired();

            builder.Property(p => p.AppUserId)
                .IsRequired();

            builder.Property(p => p.Url)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.IsMain)
                .IsRequired();

            builder.HasOne(p => p.AppUser)
                .WithMany(a => a.Photos)
                .HasForeignKey(p => p.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}