using Aris.ServerTest.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aris.ServerTest.Services
{
    public class KoreApiUserService : KoreApiBase, IKoreApiUserService
    {
        private readonly KoreOptions _koreOptions;
        private readonly IHttpClientFactory _httpClientFactory;

        public KoreApiUserService(IOptions<KoreOptions> koreOptions, IHttpClientFactory clientFactory)
        {
            _koreOptions = koreOptions.Value;
            _httpClientFactory = clientFactory;
        }

        private HttpClient GetClient(KoreAuthToken token, string returnUrl)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("X-Kore-ReturnUrl", returnUrl);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);
            return client;
        }

        public async Task<KoreUserLinks> GetUserLinksAsync(KoreAuthToken token, string returnUrl)
        {
            var client = GetClient(token, returnUrl);

            var fullUrl = new Uri(_koreOptions.BaseUrl, "users");
            var response = await client.GetAsync(fullUrl.ToString());

            await CheckResponseForErrorAsync(response);

            var data = await response.Content.ReadAsStringAsync();
            var links = JsonConvert.DeserializeObject<KoreUserLinks>(data);

            return links;
        }
    }
}
