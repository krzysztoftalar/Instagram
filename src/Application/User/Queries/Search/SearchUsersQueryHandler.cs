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
                .ToListAsync();

            var usersList = new List<UserDto>();

            foreach (var user in users)
            {
                usersList.Add(new UserDto
                {
                    Username = user.UserName,
                    DisplayName = user.DisplayName,
                    Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
                });
            }

            return usersList;
        }
    }
}
