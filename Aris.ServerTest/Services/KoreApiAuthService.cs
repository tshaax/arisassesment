using Aris.ServerTest.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Aris.ServerTest.Services
{
    public class KoreApiAuthService : KoreApiBase, IKoreApiAuthService
    {
        private readonly KoreOptions _koreOptions;
        private readonly IHttpClientFactory _httpClientFactory;

        public KoreApiAuthService(IOptions<KoreOptions> koreOptions, IHttpClientFactory clientFactory)
        {
            _koreOptions = koreOptions.Value;
            _httpClientFactory = clientFactory;
        }

        private HttpClient GetClient(string returnUrl)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("X-Kore-ReturnUrl", returnUrl);
            return client;
        }

        public async Task<Models.KoreAuthToken> GetPublicBrandTokenAsync(string returnUrl, string brand, string locale)
        {
            var client = GetClient(returnUrl);

            var fullUrl = new Uri(_koreOptions.BaseUrl, "auth/brand_public_credentials/token");
            var request = new Models.KoreAuthRequest() { Brand = brand, Locale = locale };
            var message = await client.PostAsync(fullUrl.ToString(), SerializeAuthRequest(request));

            await CheckResponseForErrorAsync(message);

            var data = await message.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<Models.KoreAuthResponse>(data);
            var token = response.Tokens.First();
            token.GrantType = response.GrantType;
            token.TrackingId = response.TrackingId;
            return token;
        }

        public async Task<Models.KoreAuthToken> GetPlayerTokenAsync(string returnUrl, string brand, string locale, string email, string password)
        {
            throw new NotImplementedException();
        }

        protected HttpContent SerializeAuthRequest(Models.KoreAuthRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
