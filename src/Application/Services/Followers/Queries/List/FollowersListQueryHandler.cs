using Application.Helpers;
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
            var queryable = request.Predicate switch
            {
                "followers" => _context.Followings.Include(x => x.Observer)
                  .ThenInclude(x => x.Photos)
                  .Where(x => x.Target.UserName == request.Username)
                  .Select(x => new FollowerDto
                  {
                      DisplayName = x.Observer.DisplayName,
                      UserName = x.Observer.UserName,
                      Image = x.Observer.Photos.FirstOrDefault(y => y.IsMain).Url
                  })
                  .AsQueryable(),

                "following" => _context.Followings.Include(x => x.Target)
                  .ThenInclude(x => x.Photos)
                  .Where(x => x.Observer.UserName == request.Username)
                  .Select(x => new FollowerDto
                  {
                      DisplayName = x.Target.DisplayName,
                      UserName = x.Target.UserName,
                      Image = x.Target.Photos.FirstOrDefault(y => y.IsMain).Url
                  })
                  .AsQueryable(),

                _ => default
            };

            var followers = await PagedList<FollowerDto>.CreateAsync(queryable, request.Skip, request.Limit);

            return new FollowersEnvelope
            {
                Followers = followers,
                FollowersCount = followers.TotalCount
            };
        }
    }
}