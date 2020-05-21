using DesktopUI.Library.Models.DbModels;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace DesktopUI.Library.Helpers
{
    public class ChatHelper : IChatHelper
    {
        private readonly IAuthenticatedUser _user;
        public HubConnection Connection { get; private set; }
        public event EventHandler<Comment> GetReceive;

        public ChatHelper(IAuthenticatedUser user)
        {
            _user = user;
        }

        public async Task CreateHubConnectionAsync(string photoId)
        {
            string api = ConfigurationManager.AppSettings["Chat"];

            Connection = new HubConnectionBuilder()
              .WithUrl(api, options =>
              {
                  options.AccessTokenProvider = () => Task.FromResult(_user.Token);
              })
              .WithAutomaticReconnect()
              .ConfigureLogging(logging => { logging.AddConsole(); })
              .Build();

            Connection.On<Comment>("ReceiveCommentAsync", (comment) =>
            {
                GetReceive?.Invoke(this, comment);
            });

            await Connection.StartAsync();

            if (Connection.State == HubConnectionState.Connected)
            {
                await Connection.InvokeAsync("AddToGroupAsync", photoId);
            }
        }

        public async Task StopHubConnectionAsync(string photoId)
        {
            await Connection.InvokeAsync("RemoveFromGroupAsync", photoId);

            await Connection.StopAsync();
        }
    }
}
