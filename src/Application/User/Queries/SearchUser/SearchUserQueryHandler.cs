using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User.Queries.SearchUser
{
    public class SearchUserQueryHandler : IRequestHandler<SearchUserQuery, UserDto>
    {
        private readonly IApplicationDbContext _context;

        public SearchUserQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> Handle(SearchUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.DisplayName == request.DisplayName);

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Username = user.UserName,
                Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
            };
        }
    }
}
