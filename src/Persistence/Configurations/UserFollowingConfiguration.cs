using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserFollowingConfiguration : IEntityTypeConfiguration<UserFollowing>
    {
        public void Configure(EntityTypeBuilder<UserFollowing> builder)
        {
            builder.HasKey(k => new { k.ObserverId, k.TargetId });

            builder.HasOne(o => o.Observer)
                .WithMany(f => f.Followings)
                .HasForeignKey(o => o.ObserverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Target)
              .WithMany(f => f.Followers)
              .HasForeignKey(o => o.TargetId)
              .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
