using MediatR;

namespace Application.Followers.Commands.Delete
{
    public class DeleteFollowersCommand : IRequest
    {
        public string Username { get; set; }
    }
}
