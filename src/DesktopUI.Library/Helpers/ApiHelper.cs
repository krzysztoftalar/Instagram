using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DesktopUI.Library.Helpers
{
    public class ApiHelper : IApiHelper
    {
        public HttpClient ApiClient { get; private set; }

        public ApiHelper()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["Api"];

            ApiClient = new HttpClient {BaseAddress = new Uri(api)};
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}