using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace DesktopUI.Library.Api.ApiResponse
{
    public static class LoginErrorResponse
    {
        public static async Task<string> LoginExceptionAsync(this HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            if (responseContent == string.Empty)
            {
                return response.Exceptions();
            }

            var exceptionResponse = JsonConvert.DeserializeObject<ApiError>(responseContent);

            return $"{exceptionResponse.Errors.User}";
        }

        public static string Exceptions(this HttpResponseMessage response)
        {
            if (response.ReasonPhrase == "Unauthorized")
            {
                return "Incorrect email address or password, please try again.";
            }

            return null;
        }

        public class ApiError
        {
            public Errors Errors { get; set; }
        }

        public class Errors
        {
            public string User { get; set; }
        }
    }
}