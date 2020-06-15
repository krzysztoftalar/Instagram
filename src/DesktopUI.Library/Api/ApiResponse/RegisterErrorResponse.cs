using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace DesktopUI.Library.Api.ApiResponse
{
    public static class RegisterErrorResponse
    {
        public static async Task<string> RegisterExceptionAsync(this HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            var exceptionResponse = JsonConvert.DeserializeObject<ApiError>(responseContent);

            return $"{exceptionResponse.Errors.UserName}{exceptionResponse.Errors.Email}";
        }

        public class ApiError
        {
            public Errors Errors { get; set; }
        }

        public class Errors
        {
            public string Email { get; set; }
            public string UserName { get; set; }
        }
    }
}