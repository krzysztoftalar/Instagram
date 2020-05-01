using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.User.Queries.CurrentUser
{
    public class CurrentUserQueryHandler : IRequestHandler<CurrentUserQuery, CurrentUserDto>
    {
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IUserAccessor _userAccessor;
        private readonly IApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CurrentUserQueryHandler(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator,
            IUserAccessor userAccessor, IApplicationDbContext context)
        {
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _userAccessor = userAccessor;
            _context = context;
        }

        public async Task<CurrentUserDto> Handle(CurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

            var photo = await _context.Photos
                .FirstOrDefaultAsync(x => x.AppUserId == user.Id && x.IsMain, cancellationToken: cancellationToken);

            return new CurrentUserDto
            {
                DisplayName = user.DisplayName,
                Username = user.UserName,
                Token = _jwtGenerator.CreateToken(user),
                Image = photo?.Url
            };
        }
    }
}