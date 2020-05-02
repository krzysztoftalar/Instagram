using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Followers.Queries.List
{
    public class FollowersListQueryHandler : IRequestHandler<FollowersListQuery, List<FollowersProfileDto>>
    {
        private readonly IApplicationDbContext _context;

        public FollowersListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FollowersProfileDto>> Handle(FollowersListQuery request, CancellationToken cancellationToken)
        {
            var profiles = new List<FollowersProfileDto>();

            switch (request.Predicate)
            {
                case "followers":
                {
                    profiles = await _context.Followings
                        .Where(x => x.Target.UserName == request.Username)
                        .Include(x => x.Observer)
                        .ThenInclude(x => x.Photos)
                        .Select(x => new FollowersProfileDto
                        {
                            DisplayName = x.Observer.DisplayName,
                            UserName = x.Observer.UserName,
                            Image = x.Observer.Photos.FirstOrDefault(y => y.IsMain).Url
                        })
                        .ToListAsync(cancellationToken);

                    break;
                }

                case "following":
                {
                    profiles = await _context.Followings
                        .Where(x => x.Observer.UserName == request.Username)
                        .Include(x => x.Target)
                        .ThenInclude(x => x.Photos)
                        .Select(x => new FollowersProfileDto
                        {
                            DisplayName = x.Target.DisplayName,
                            UserName = x.Target.UserName,
                            Image = x.Target.Photos.FirstOrDefault(y => y.IsMain).Url
                        })
                        .ToListAsync(cancellationToken);

                    break;
                }
            }

            return profiles;
        }
    }
}