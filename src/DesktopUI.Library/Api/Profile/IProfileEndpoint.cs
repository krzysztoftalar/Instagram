using DesktopUI.Library.Models;
using DesktopUI.Library.Models.DbModels;
using System.Threading.Tasks;

namespace DesktopUI.Library.Api.Profile
{
    public interface IProfileEndpoint
    {
        Task<bool> UploadPhoto(string photo);
        Task<bool> SetMainPhoto(Photo photo);
        Task<bool> DeletePhoto(Photo photo);
        Task<PhotosEnvelope> LoadPhotos(string username, int? skip, int? limit);

        Task<Models.DbModels.Profile> LoadProfile(string username);
        Task<bool> EditProfile(ProfileFormValues profile);

        Task<FollowersEnvelope> LoadFollowing(string username, string predicate, int? skip, int? limit);
        Task Follow(string username);
        Task UnFollow(string username);
    }
}