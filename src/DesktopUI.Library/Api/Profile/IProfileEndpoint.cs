using DesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesktopUI.Library.Api.Profile
{
    public interface IProfileEndpoint
    {
        Task UploadPhoto(string photo);
        Task SetMainPhoto(Photo photo);
        Task DeletePhoto(Photo photo);

        Task<Models.Profile> LoadProfile(string username);
        Task EditProfile(ProfileFormValues profile);

        Task<FollowersEnvelope> LoadFollowing(string username, string predicate, int? skip, int? limit);
        Task Follow(string username);
        Task UnFollow(string username);
    }
}