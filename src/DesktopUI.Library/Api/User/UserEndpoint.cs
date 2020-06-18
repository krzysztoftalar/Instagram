using DesktopUI.Library.Helpers;
using DesktopUI.Library.Models;
using DesktopUI.Library.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using DesktopUI.Library.Errors;

namespace DesktopUI.Library.Api.User
{
    public class UserEndpoint : IUserEndpoint
    {
        private readonly IApiHelper _apiHelper;
        private readonly IAuthenticatedUser _user;

        public UserEndpoint(IApiHelper apiHelper, IAuthenticatedUser user)
        {
            _apiHelper = apiHelper;
            _user = user;
        }

        public async Task RegisterAsync(RegisterUserFormValues user)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.PostAsJsonAsync("/api/users/register", user))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(await response.ExceptionAsync());
                }
            }
        }

        public async Task<AuthenticatedUser> LoginAsync(LoginUserFormValues user)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.PostAsJsonAsync("/api/users/login", user))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<AuthenticatedUser>();
                }

                throw new Exception(await response.ExceptionAsync());
            }
        }

        public void LogOffUser()
        {
            _apiHelper.ApiClient.DefaultRequestHeaders.Clear();
        }

        public async Task<AuthenticatedUser> CurrentUserAsync(string token)
        {
            _apiHelper.ApiClient.DefaultRequestHeaders.Clear();
            _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Clear();
            _apiHelper.ApiClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _apiHelper.ApiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/users"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                    _user.DisplayName = result.DisplayName;
                    _user.Username = result.Username;
                    _user.Image = result.Image;
                    _user.Token = result.Token;
                    return result;
                }

                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<List<AuthenticatedUser>> SearchUsersAsync(string displayName)
        {
            using (HttpResponseMessage response =
                await _apiHelper.ApiClient.GetAsync($"/api/users/{displayName}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<List<AuthenticatedUser>>();
                }

                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task VerifyEmail(string userId, string emailToken)
        {
            string code = HttpUtility.UrlEncode(emailToken);

            using (HttpResponseMessage response =
              await _apiHelper.ApiClient.GetAsync($"/api/users/verify/email?userId={userId}&emailToken={code}"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(await response.ExceptionAsync());
                }
            }
        }
    }
}