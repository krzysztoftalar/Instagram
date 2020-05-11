using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            var profile = await _context.Users
                .Include(x => x.Photos)
                .Include(x => x.Followers)
                .Include(x => x.Followings)
                .Where(x => x.UserName == request.Username)
                .ProjectTo<ProfileDto>(_mapper.ConfigurationProvider)
                .SingleAsync(cancellationToken);

            if (profile == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new {User = "Not Found"});
            }

            var currentUser = await _context.Users
                .Include(x => x.Followings)
                .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetCurrentUsername(),
                    cancellationToken: cancellationToken);

            if (currentUser.Followings.Any(x => x.TargetId == profile.Id))
            {
                profile.Following = true;
            }

            return profile;
        }
    }
}