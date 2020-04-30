using MediatR;

namespace Application.Services.Followers.Commands.Add
{
    public class AddFollowersCommand : IRequest
    {
        public string Username { get; set; }
    }
}
