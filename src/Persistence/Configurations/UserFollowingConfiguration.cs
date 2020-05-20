using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class UserFollowingConfiguration : IEntityTypeConfiguration<UserFollowing>
    {
        public void Configure(EntityTypeBuilder<UserFollowing> builder)
        {
            builder.HasKey(u => new { u.ObserverId, u.TargetId });

            builder.HasOne(u => u.Observer)
                .WithMany(a => a.Followings)
                .HasForeignKey(u => u.ObserverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Target)
                .WithMany(a => a.Followers)
                .HasForeignKey(u => u.TargetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasCheckConstraint("CK_UserFollowing_TargetId", "[TargetId] <> [ObserverId]");
        }
    }
}