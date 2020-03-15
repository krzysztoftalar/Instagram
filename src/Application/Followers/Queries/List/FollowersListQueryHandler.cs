using Application.Interfaces;
using Application.Profiles;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Followers.Queries.List
{
    public class FollowersListQueryHandler : IRequestHandler<FollowersListQuery, List<Profile>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IProfileReader _profileReader;

        public FollowersListQueryHandler(IApplicationDbContext context, IProfileReader profileReader)
        {
            _context = context;
            _profileReader = profileReader;
        }

        public async Task<List<Profile>> Handle(FollowersListQuery request, CancellationToken cancellationToken)
        {
            var queryable = _context.Followings.AsQueryable();

            var userFollowings = new List<UserFollowing>();
            var profiles = new List<Profile>();

            switch (request.Predicate)
            {
                case "followers":
                    {
                        userFollowings = await queryable
                            .Where(x => x.Target.UserName == request.Username)
                            .ToListAsync();

                        foreach (var follower in userFollowings)
                        {
                            profiles.Add(await _profileReader.ReadProfile(follower.Observer.UserName));
                        }

                        break;
                    }

                case "following":
                    {
                        userFollowings = await queryable
                            .Where(x => x.Observer.UserName == request.Username)
                            .ToListAsync();

                        foreach (var follower in userFollowings)
                        {
                            profiles.Add(await _profileReader.ReadProfile(follower.Target.UserName));
                        }

                        break;
                    }
            }

            return profiles;
        }
    }
}
