using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.User.Queries.Login
{
    public class LoginUserQuery : IRequest<UserDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
