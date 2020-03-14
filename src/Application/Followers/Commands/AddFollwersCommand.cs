using MediatR;

namespace Application.Followers.Commands
{
    public class AddFollwersCommand : IRequest
    {
        public string Username { get; set; }
    }
}
