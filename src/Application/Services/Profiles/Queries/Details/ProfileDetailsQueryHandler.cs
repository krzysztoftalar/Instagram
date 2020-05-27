using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

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
            var profile = await _context.Users
                .Include(x => x.Photos)
                .Include(x => x.Followers)
                .Include(x => x.Followings)
                .Where(x => x.UserName == request.Username)
                .ProjectTo<ProfileDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (profile == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new { User = "Not Found" });
            }

            var isFollowing = await _context.Users
                .Include(x => x.Followings)
                .AsNoTracking()
                .SelectMany(x => x.Followings)
                .AnyAsync(x => x.TargetId == profile.Id, cancellationToken);

            if (isFollowing)
            {
                profile.Following = true;
            }

            return profile;
        }
    }
}