using Application.Errors;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Application.Profiles
{
    public class ProfileReader : IProfileReader
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserAccessor _userAccessor;

        public ProfileReader(IApplicationDbContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Profile> ReadProfile(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);

            if (user == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { User = "Not Found" });
            }

            var currentUser = await _context.Users.SingleOrDefaultAsync(x =>
               x.UserName == _userAccessor.GetCurrentUsername());

            var profile = new Profile
            {
                DisplayName = user.DisplayName,
                Username = user.UserName,
                Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                Photos = user.Photos,
                FollowingCount = user.Followings.Count(),
                FollowersCount = user.Followers.Count()
            };

            if (currentUser.Followings.Any(x => x.TargetId == user.Id))
            {
                profile.Following = true;
            }

            return profile;
        }
    }
}
