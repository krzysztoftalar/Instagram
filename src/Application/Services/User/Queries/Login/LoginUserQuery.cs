﻿using MediatR;

namespace Application.Services.User.Queries.Login
{
    public class LoginUserQuery : IRequest<LoginUserDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
