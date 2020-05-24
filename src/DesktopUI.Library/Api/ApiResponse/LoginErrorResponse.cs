using System.Net.Http;

namespace DesktopUI.Library.Api.ApiResponse
{
    public static class LoginErrorResponse
    {
        public static string LoginException(this HttpResponseMessage response)
        {
            if (response.ReasonPhrase == "Unauthorized")
            {
                return "Incorrect email address or password, please try again.";
            }

            return null;
        }
    }
}