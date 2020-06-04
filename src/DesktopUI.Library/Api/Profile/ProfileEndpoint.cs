using DesktopUI.Library.Helpers;
using DesktopUI.Library.Models;
using DesktopUI.Library.Models.DbModels;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;

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

        public async Task<bool> UploadPhotoAsync(string photo)
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

                                    return true;
                                }

                                throw new Exception(response.ReasonPhrase);
                            }
                        }
                    }
                }
            }
        }

        public async Task<bool> SetMainPhotoAsync(Photo photo)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.PostAsJsonAsync($"/api/photos/{photo.Id}/setMain",
                    new StringContent(string.Empty)))
            {
                if (response.IsSuccessStatusCode)
                {
                    _profile.Image = photo.Url;
                    _user.Image = photo.Url;
                    return true;
                }

                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<bool> DeletePhotoAsync(Photo photo)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.DeleteAsync($"/api/photos/{photo.Id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    if (photo.IsMain)
                    {
                        _profile.Image = null;
                        _user.Image = null;
                    }

                    return true;
                }

                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<PhotosEnvelope> LoadPhotosAsync(string username, int? skip, int? limit)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.GetAsync($"/api/photos/{username}?skip={skip}&limit={limit}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<PhotosEnvelope>();
                }

                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<Models.DbModels.Profile> LoadProfileAsync(string username)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.GetAsync($"/api/profiles/{username}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<Models.DbModels.Profile>();    
                    _profile.DisplayName = result.DisplayName;
                    _profile.Username = result.Username;
                    _profile.Bio = result.Bio;
                    _profile.Image = result.Image;
                    _profile.Following = result.Following;
                    _profile.FollowingCount = result.FollowingCount;
                    _profile.FollowersCount = result.FollowersCount;
                    return result;
                }

                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<bool> EditProfileAsync(ProfileFormValues profile)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.PutAsJsonAsync($"/api/profiles", profile))
            {
                if (response.IsSuccessStatusCode)
                {
                    _profile.DisplayName = profile.DisplayName;
                    _user.DisplayName = profile.DisplayName;
                    _profile.Bio = profile.Bio;
                    return true;
                }

                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task FollowAsync(string username)
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

        public async Task UnFollowAsync(string username)
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

        public async Task<FollowersEnvelope> LoadFollowingAsync(string username, string predicate, int? skip, int? limit)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient
                    .GetAsync($"/api/profiles/{username}/follow?predicate={predicate}&skip={skip}&limit={limit}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<FollowersEnvelope>();
                }

                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}