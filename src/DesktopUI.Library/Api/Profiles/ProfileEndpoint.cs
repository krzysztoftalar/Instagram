using DesktopUI.Library.Helpers;
using DesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DesktopUI.Library.Api.Profiles
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

        public async Task UpoloadPhoto(string photo)
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
                                    _user.Image = result.Url;
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

        public async Task<Profile> LoadProfile(string username)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.GetAsync($"/api/profiles/{username}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<Profile>();
                    _profile.DisplayName = result.DisplayName;
                    _profile.Username = result.Username;
                    _profile.Image = result.Image;
                    _profile.Photos = result.Photos;
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

        public async Task Follow(string username)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.PostAsJsonAsync($"/api/profiles/{username}/follow",
                new StringContent(string.Empty)))
            {
                if (response.IsSuccessStatusCode)
                {
                    _profile.Following = true;
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
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<List<Profile>> LoadFollowing(string username, string predicate)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.GetAsync($"/api/profiles/{username}/follow?predicate={predicate}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<Profile>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}

