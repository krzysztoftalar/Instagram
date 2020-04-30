using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DesktopUI.Library.Helpers
{
    public class ApiHelper : IApiHelper
    {
        private HttpClient _apiClient;

        public HttpClient ApiClient => _apiClient;

        public ApiHelper()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["Api"];

            _apiClient = new HttpClient {BaseAddress = new Uri(api)};
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}