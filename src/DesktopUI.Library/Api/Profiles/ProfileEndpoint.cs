using DesktopUI.Library.Helpers;
using DesktopUI.Library.Models;
using System;
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

        public ProfileEndpoint(IApiHelper apiHelper, IProfile profile)
        {
            _apiHelper = apiHelper;
            _profile = profile;
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
                                if (response.IsSuccessStatusCode == false)
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
                    _profile.FollowingsCount = result.FollowingsCount;
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
                await _apiHelper.ApiClient.PostAsJsonAsync($"/api/profiles/{username}/follow", new StringContent(string.Empty)))
            {
                if (response.IsSuccessStatusCode)
                {
                    _profile.Following = true;
                    _profile.FollowingsCount++;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}

