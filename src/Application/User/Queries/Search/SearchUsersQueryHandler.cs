using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User.Queries.Search
{
    public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, List<UserDto>>
    {
        private readonly IApplicationDbContext _context;

        public SearchUsersQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDto>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.Users
                .Where(x => x.DisplayName == request.DisplayName)
                .Select(x => new UserDto
                {
                    DisplayName = x.DisplayName,
                    Username = x.UserName,
                    Image = x.Photos.FirstOrDefault(x => x.IsMain).Url
                })
                .ToListAsync();

            return users;
        }
    }
}
