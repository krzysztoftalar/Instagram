using DesktopUI.Library.Models;
using DesktopUI.Library.Models.DbModels;
using System.Threading.Tasks;

namespace DesktopUI.Library.Api.Profile
{
    public interface IProfileEndpoint
    {
        Task<bool> UploadPhotoAsync(string photo);
        Task<bool> SetMainPhotoAsync(Photo photo);
        Task<bool> DeletePhotoAsync(Photo photo);
        Task<PhotosEnvelope> LoadPhotosAsync(string username, int? skip, int? limit);

        Task<Models.DbModels.Profile> LoadProfileAsync(string username);
        Task<bool> EditProfileAsync(ProfileFormValues profile);

        Task<FollowersEnvelope> LoadFollowingAsync(string username, string predicate, int? skip, int? limit);
        Task FollowAsync(string username);
        Task UnFollowAsync(string username);
    }
}