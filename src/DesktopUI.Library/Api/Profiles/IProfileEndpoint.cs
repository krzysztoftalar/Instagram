using DesktopUI.Library.Models;
using System.Threading.Tasks;

namespace DesktopUI.Library.Api.Profiles
{
    public interface IProfileEndpoint
    {
        Task UpoloadPhoto(string photo);
        Task<Profile> LoadProfile(string username);
        Task Follow(string username);
    }
}