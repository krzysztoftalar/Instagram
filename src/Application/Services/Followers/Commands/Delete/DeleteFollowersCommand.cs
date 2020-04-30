using MediatR;

namespace Application.Services.Followers.Commands.Delete
{
    public class DeleteFollowersCommand : IRequest
    {
        public string Username { get; set; }
    }
}
