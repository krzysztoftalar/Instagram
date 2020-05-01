using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.User.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly UserManager<AppUser> _userManager;

        public RegisterUserCommandHandler(IApplicationDbContext context, IJwtGenerator jwtGenerator, UserManager<AppUser> userManager)
        {
            _context = context;
            _jwtGenerator = jwtGenerator;
            _userManager = userManager;
        }

        public async Task<RegisterUserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _context.Users.Where(x => x.Email == request.Email).AnyAsync(cancellationToken: cancellationToken))
            {
                throw new RestException(HttpStatusCode.BadRequest, new {Email = "Email already exists"});
            }

            if (await _context.Users.Where(x => x.UserName == request.Username).AnyAsync(cancellationToken: cancellationToken))
            {
                throw new RestException(HttpStatusCode.BadRequest, new {UserName = "UserName already exists"});
            }

            var user = new AppUser
            {
                DisplayName = request.DisplayName,
                UserName = request.Username,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return new RegisterUserDto
                {
                    DisplayName = user.DisplayName,
                    Username = user.UserName,
                    Token = _jwtGenerator.CreateToken(user)
                };
            }

            throw new Exception("Problem creating user");
        }
    }
}

