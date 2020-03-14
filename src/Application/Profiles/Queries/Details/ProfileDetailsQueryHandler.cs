using Application.Errors;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Profiles.Queries.Details
{
    public class ProfileDetailsQueryHandler : IRequestHandler<ProfileDetailsQuery, Profile>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserAccessor _userAccessor;

        public ProfileDetailsQueryHandler(IApplicationDbContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Profile> Handle(ProfileDetailsQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == request.Username);

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
                FollowingsCount = user.Followings.Count()
            };

            if (currentUser.Followings.Any(x => x.TargetId == user.Id))
            {
                profile.Following = true;
            }

            return profile;
        }
    }
}
