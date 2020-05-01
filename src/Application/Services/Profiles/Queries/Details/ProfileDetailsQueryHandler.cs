using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Profiles.Queries.Details
{
    public class ProfileDetailsQueryHandler : IRequestHandler<ProfileDetailsQuery, ProfileDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public ProfileDetailsQueryHandler(IApplicationDbContext context, IUserAccessor userAccessor, IMapper mapper)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }

        public async Task<ProfileDto> Handle(ProfileDetailsQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(x => x.Photos)
                .Include(x => x.Followers)
                .Include(x => x.Followings)
                .SingleOrDefaultAsync(x => x.UserName == request.Username, cancellationToken: cancellationToken);

            if (user == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new {User = "Not Found"});
            }

            var currentUser = await _context.Users
                .Include(x => x.Followings)
                .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername(),
                    cancellationToken: cancellationToken);

            var profile = new Profile
            {
                DisplayName = user.DisplayName,
                Username = user.UserName,
                Bio = user.Bio,
                Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                Photos = user.Photos,
                FollowingCount = user.Followings.Count(),
                FollowersCount = user.Followers.Count()
            };

            if (currentUser.Followings.Any(x => x.TargetId == user.Id))
            {
                profile.Following = true;
            }

            return _mapper.Map<ProfileDto>(profile);
        }
    }
}