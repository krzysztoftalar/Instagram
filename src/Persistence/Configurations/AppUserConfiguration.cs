using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(a => a.DisplayName)
                .HasMaxLength(50)
                .IsRequired();
            
            builder.Property(a => a.Bio)
                .IsRequired(false);
        }
    }
}