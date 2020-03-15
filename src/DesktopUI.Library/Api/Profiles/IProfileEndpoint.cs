using DesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesktopUI.Library.Api.Profiles
{
    public interface IProfileEndpoint
    {
        Task UpoloadPhoto(string photo);
        Task<Profile> LoadProfile(string username);
        Task<List<Profile>> LoadFollowing(string username, string predicate);
        Task Follow(string username);
        Task UnFollow(string username);
    }
}