using MediatR;

namespace Application.Followers.Commands.Add
{
    public class AddFollwersCommand : IRequest
    {
        public string Username { get; set; }
    }
}
