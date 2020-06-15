using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DesktopUI.Library.Helpers
{
    public class ApiHelper : IApiHelper
    {
        private readonly IConfiguration _configuration;
        public HttpClient ApiClient { get; private set; }

        public ApiHelper(IConfiguration configuration)
        {
            _configuration = configuration;

            InitializeClient();
        }

        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["Api"] ?? _configuration["ServerSettings:Url"];

            ApiClient = new HttpClient { BaseAddress = new Uri(api) };
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}