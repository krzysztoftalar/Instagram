using DesktopUI.Library.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using DesktopUI.Library.Models.DbModels;

namespace DesktopUI.Library.Helpers
{
    public interface IChatHelper
    {
        HubConnection Connection { get; }
        Task CreateHubConnection(string photoId);
        Task StopHubConnection(string photoId);

        event EventHandler<Comment> GetReceive;
    }
}
