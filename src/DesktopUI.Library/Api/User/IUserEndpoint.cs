using DesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DesktopUI.Library.Models.DbModels;

namespace DesktopUI.Library.Api.User
{
    public interface IUserEndpoint
    {
        Task Register(UserFormValues data);
        Task<AuthenticatedUser> Login(UserFormValues user);
        void LogOffUser();
        Task<AuthenticatedUser> CurrentUser(string token);
        Task<List<AuthenticatedUser>> SearchUsers(string displayName);
    }
}