using DesktopUI.Library.Models;
using System;
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
        Task<List<Models.Profile>> LoadFollowing(string username, string predicate);

        Task Follow(string username);
        Task UnFollow(string username);
    }
}