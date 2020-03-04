using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.User.Queries.CurrentUser
{
    public class CurrentUserQueryHandler : IRequestHandler<CurrentUserQuery, UserDto>
    {
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IUserAccessor _usserAccessor;
        private readonly UserManager<AppUser> _userManager;

        public CurrentUserQueryHandler(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator,
            IUserAccessor usserAccessor)
        {
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _usserAccessor = usserAccessor;
        }

        public async Task<UserDto> Handle(CurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(_usserAccessor.GetCurrentUsername());

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = _jwtGenerator.CreateToken(user),
                Username = user.UserName,
                Image = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
            };
        }
    }
}
