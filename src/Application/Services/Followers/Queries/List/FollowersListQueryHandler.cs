using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Followers.Queries.List
{
    public class FollowersListQueryHandler : IRequestHandler<FollowersListQuery.Query, FollowersEnvelope>
    {
        private readonly IApplicationDbContext _context;

        public FollowersListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FollowersEnvelope> Handle(FollowersListQuery.Query request, CancellationToken cancellationToken)
        {
            var followers = default(FollowersEnvelope);

            switch (request.Predicate)
            {
                case "followers":
                    {
                        var queryable = _context.Followings
                            .Include(x => x.Observer)
                            .ThenInclude(x => x.Photos)
                            .Where(x => x.Target.UserName == request.Username)
                            .Select(x => new FollowerDto
                            {
                                DisplayName = x.Observer.DisplayName,
                                UserName = x.Observer.UserName,
                                Image = x.Observer.Photos.FirstOrDefault(y => y.IsMain).Url
                            }).AsQueryable();

                        followers = new FollowersEnvelope
                        {
                            Followers = await queryable
                                .Skip(request.Skip ?? 0)
                                .Take(request.Limit ?? 10)
                                .ToListAsync(cancellationToken),
                            FollowersCount = await queryable.CountAsync(cancellationToken)
                        };

                        break;
                    }

                case "following":
                    {
                        var queryable = _context.Followings
                            .Include(x => x.Target)
                            .ThenInclude(x => x.Photos)
                            .Where(x => x.Observer.UserName == request.Username)
                            .Select(x => new FollowerDto
                            {
                                DisplayName = x.Target.DisplayName,
                                UserName = x.Target.UserName,
                                Image = x.Target.Photos.FirstOrDefault(y => y.IsMain).Url
                            }).AsQueryable();

                        followers = new FollowersEnvelope
                        {
                            Followers = await queryable
                                .Skip(request.Skip ?? 0)
                                .Take(request.Limit ?? 10)
                                .ToListAsync(cancellationToken),
                            FollowersCount = await queryable.CountAsync(cancellationToken)
                        };

                        break;
                    }
            }

            return followers;
        }
    }
}