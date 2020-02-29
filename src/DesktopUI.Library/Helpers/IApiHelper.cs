using System.Net.Http;

namespace DesktopUI.Library.Helpers
{
    public interface IApiHelper
    {
        HttpClient ApiClient { get; }
    }
}