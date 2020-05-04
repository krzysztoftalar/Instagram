using DesktopUI.Library.Helpers;
using DesktopUI.Library.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DesktopUI.Library.Api.Profile
{
    public class ProfileEndpoint : IProfileEndpoint
    {
        private readonly IApiHelper _apiHelper;
        private readonly IProfile _profile;
        private readonly IAuthenticatedUser _user;

        public ProfileEndpoint(IApiHelper apiHelper, IProfile profile, IAuthenticatedUser user)
        {
            _apiHelper = apiHelper;
            _profile = profile;
            _user = user;
        }

        public async Task UploadPhoto(string photo)
        {
            using (var form = new MultipartFormDataContent())
            {
                using (var fs = File.OpenRead(photo))
                {
                    using (var streamContent = new StreamContent(fs))
                    {
                        using (var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync()))
                        {
                            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                            form.Add(fileContent, "File", Path.GetFileName(photo));

                            using (HttpResponseMessage response =
                                await _apiHelper.ApiClient.PostAsync("/api/photos", form))
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    var result = await response.Content.ReadAsAsync<Photo>();

                                    if (result.IsMain)
                                    {
                                        _user.Image = result.Url;
                                        _profile.Image = result.Url;
                                    }
                                }
                                else
                                {
                                    throw new Exception(response.ReasonPhrase);
                                }
                            }
                        }
                    }
                }
            }
        }

        public async Task SetMainPhoto(Photo photo)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.PostAsJsonAsync($"/api/photos/{photo.Id}/setMain",
                    new StringContent(string.Empty)))
            {
                if (response.IsSuccessStatusCode)
                {
                    _profile.Image = photo.Url;
                    _user.Image = photo.Url;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task DeletePhoto(Photo photo)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.DeleteAsync($"/api/photos/{photo.Id}"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<PhotosEnvelope> LoadPhotos(string username, int? skip, int? limit)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.GetAsync($"/api/photos/{username}?skip={skip}&limit={limit}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<PhotosEnvelope>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<Models.Profile> LoadProfile(string username)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.GetAsync($"/api/profiles/{username}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<Models.Profile>();
                    _profile.DisplayName = result.DisplayName;
                    _profile.Username = result.Username;
                    _profile.Bio = result.Bio;
                    _profile.Image = result.Image;
                    _profile.Following = result.Following;
                    _profile.FollowingCount = result.FollowingCount;
                    _profile.FollowersCount = result.FollowersCount;
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task EditProfile(ProfileFormValues profile)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.PutAsJsonAsync($"/api/profiles", profile))
            {
                if (response.IsSuccessStatusCode)
                {
                    _profile.DisplayName = profile.DisplayName;
                    _user.DisplayName = profile.DisplayName;
                    _profile.Bio = profile.Bio;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Follow(string username)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.PostAsJsonAsync($"/api/profiles/{username}/follow",
                    new StringContent(string.Empty)))
            {
                if (response.IsSuccessStatusCode)
                {
                    _profile.Following = true;
                    _profile.FollowersCount++;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task UnFollow(string username)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.DeleteAsync($"/api/profiles/{username}/follow"))
            {
                if (response.IsSuccessStatusCode)
                {
                    _profile.Following = false;
                    _profile.FollowersCount--;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<FollowersEnvelope> LoadFollowing(string username, string predicate, int? skip, int? limit)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient
                    .GetAsync($"/api/profiles/{username}/follow?predicate={predicate}&skip={skip}&limit={limit}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<FollowersEnvelope>();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}