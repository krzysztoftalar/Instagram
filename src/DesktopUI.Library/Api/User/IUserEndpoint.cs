using DesktopUI.Library.Models;
using DesktopUI.Library.Models.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesktopUI.Library.Api.User
{
    public interface IUserEndpoint
    {
        Task RegisterAsync(RegisterUserFormValues data);
        Task<AuthenticatedUser> LoginAsync(LoginUserFormValues user);
        void LogOffUser();
        Task<AuthenticatedUser> CurrentUserAsync(string token);
        Task<List<AuthenticatedUser>> SearchUsersAsync(string displayName);
        Task VerifyEmail(string userId, string emailToken);
    }
}