using Application.Interfaces;
using Application.Services.Comments.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebUI.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;
        private readonly IUserAccessor _userAccessor;

        public ChatHub(IMediator mediator, IUserAccessor userAccessor)
        {
            _mediator = mediator;
            _userAccessor = userAccessor;
        }

        public async Task SendComment(CreateCommentCommand command)
        {
            string username = _userAccessor.GetCurrentUsername();

            command.Username = username;

            var comment = await _mediator.Send(command);

            await Clients.Group(command.PhotoId).SendAsync("ReceiveComment", comment);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var username = _userAccessor.GetCurrentUsername();

            await Clients.Group(groupName).SendAsync("Send", $"{username} has joined the group");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            var username = _userAccessor.GetCurrentUsername();

            await Clients.Group(groupName).SendAsync("Send", $"{username} has left the group");
        }
    }
}