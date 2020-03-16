using DesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesktopUI.Library.Api.Profile
{
    public interface IProfileEndpoint
    {
        Task UpoloadPhoto(string photo);
        Task<Models.Profile> LoadProfile(string username);
        Task<List<Models.Profile>> LoadFollowing(string username, string predicate);
        Task Follow(string username);
        Task UnFollow(string username);
    }
}