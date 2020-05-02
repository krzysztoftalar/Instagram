using System.Net.Http;
using Newtonsoft.Json;

namespace DesktopUI.Library.Api.ApiResponse
{
    public static class ApiErrorExtensions
    {
        public static string ApiException(this HttpResponseMessage response)
        {
            var responseContent = response.Content.ReadAsStringAsync().Result;
            
            var exceptionResponse = JsonConvert.DeserializeObject<ApiError>(responseContent);

            return $"{exceptionResponse.Errors.Email}{exceptionResponse.Errors.UserName}";
        }
        
        public class ApiError
        {
            [JsonProperty("errors")]         

            public Errors Errors { get; set; }
        }

        public class Errors
        {
            [JsonProperty("Email")]
            public string Email { get; set; }
            [JsonProperty("UserName")]
            public string UserName { get; set; }
        }
    }
}