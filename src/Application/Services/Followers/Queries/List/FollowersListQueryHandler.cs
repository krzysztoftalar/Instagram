using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Followers.Queries.List
{
    public class FollowersListQueryHandler : IRequestHandler<FollowersListQuery, List<ProfileDto>>
    {
        private readonly IApplicationDbContext _context;

        public FollowersListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProfileDto>> Handle(FollowersListQuery request, CancellationToken cancellationToken)
        {
            var userFollowings = new List<UserFollowing>();
            var profiles = new List<ProfileDto>();

            switch (request.Predicate)
            {
                case "followers":
                    {
                        userFollowings = await _context.Followings
                            .Include(x => x.Observer).ThenInclude(x => x.Photos)
                            .Include(x => x.Target).ThenInclude(x => x.Photos)
                            .Where(x => x.Target.UserName == request.Username)
                            .ToListAsync(cancellationToken: cancellationToken);

                        foreach (var follower in userFollowings)
                        {
                            profiles.Add(new ProfileDto
                            {
                                DisplayName = follower.Observer.DisplayName,
                                UserName = follower.Observer.UserName,
                                Image = follower.Observer.Photos.FirstOrDefault(x => x.IsMain)?.Url
                            });
                        }

                        break;
                    }

                case "following":
                    {
                        userFollowings = await _context.Followings
                            .Include(x => x.Observer).ThenInclude(x => x.Photos)
                            .Include(x => x.Target).ThenInclude(x => x.Photos)
                            .Where(x => x.Observer.UserName == request.Username)
                            .ToListAsync(cancellationToken: cancellationToken);

                        foreach (var follower in userFollowings)
                        {
                            profiles.Add(new ProfileDto
                            {
                                DisplayName = follower.Target.DisplayName,
                                UserName = follower.Target.UserName,
                                Image = follower.Target.Photos.FirstOrDefault(x => x.IsMain)?.Url
                            });
                        }

                        break;
                    }
            }

            return profiles;
        }
    }
}