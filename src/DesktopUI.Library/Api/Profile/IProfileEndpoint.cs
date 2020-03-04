using System.Threading.Tasks;

namespace DesktopUI.Library.Api.Profile
{
    public interface IProfileEndpoint
    {
        Task UpoloadPhoto(string photo);
    }
}