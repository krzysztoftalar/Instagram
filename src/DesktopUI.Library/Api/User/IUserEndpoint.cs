using DesktopUI.Library.Models;
using System.Threading.Tasks;

namespace DesktopUI.Library.Api.User
{
    public interface IUserEndpoint
    {
        Task Register(UserFormValues data);
        Task<AuthenticatedUser> Login(UserFormValues user);
        Task<AuthenticatedUser> CurrentUser(string token);
    }
}