using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DesktopUI.Library.Errors
{
    public static class ErrorResponse
    {
        public static async Task<string> ExceptionAsync(this HttpResponseMessage response)
        {
            string responseContent = await response.Content.ReadAsStringAsync();

            var exceptionResponse = JsonConvert.DeserializeObject<ApiError>(responseContent);

            var errors = exceptionResponse?.Errors;

            return errors == null
                ? response.Exceptions()
                : errors.GetType()
                    .GetProperties()
                    .Where(x => x.GetValue(errors) != null)
                    .Select(x => x.GetValue(errors) as string)
                    .First();
        }

        public static string Exceptions(this HttpResponseMessage response)
        {
            if (response.ReasonPhrase == "Unauthorized")
                return "Incorrect email address or password, please try again.";

            return null;
        }
    }
}