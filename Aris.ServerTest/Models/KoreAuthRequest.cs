using Newtonsoft.Json;

namespace Aris.ServerTest.Models
{
    public class KoreAuthRequest
    {
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        [JsonProperty("brand_id")]
        public string Brand { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("play_mode")]
        public string PlayMode { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
