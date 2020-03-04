using DesktopUI.Library.Helpers;
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

        public ProfileEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
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
    }
}

