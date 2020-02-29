using MediatR;

namespace Application.User.Commands.Register
{
    public class RegisterUserCommand : IRequest<UserDto>
    {
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
