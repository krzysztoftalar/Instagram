using DesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DesktopUI.Library.Models.DbModels;

namespace DesktopUI.Library.Api.User
{
    public interface IUserEndpoint
    {
        Task RegisterAsync(RegisterUserFormValues data);
        Task<AuthenticatedUser> LoginAsync(LoginUserFormValues user);
        void LogOffUser();
        Task<AuthenticatedUser> CurrentUserAsync(string token);
        Task<List<AuthenticatedUser>> SearchUsersAsync(string displayName);
    }
}