using System;
using System.Linq;
using System.Security.Claims;

using Newtonsoft.Json;

namespace Aris.ServerTest.Models
{
    public class KoreAuthToken
    {

        public const string BrandPublic = "brand_public_credentials";
        public const string Player = "player_credentials";

        [JsonProperty("type")]
        public string TokenType { get; set; }

        public string GrantType { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("expiration")]
        public DateTime Expiration { get; set; }

        public string TrackingId { get; set; }


        public static KoreAuthToken GetAuthTokenFromClaims(ClaimsPrincipal user)
        {
            KoreAuthToken token = new KoreAuthToken();
            token.TokenType = user.Claims.FirstOrDefault(c => c.Type == "TokenType").Value;
            token.GrantType = user.Claims.FirstOrDefault(c => c.Type == "GrantType").Value;
            token.Value = user.Claims.FirstOrDefault(c => c.Type == "Value").Value;
            token.Expiration = Convert.ToDateTime(user.Claims.FirstOrDefault(c => c.Type == "Expiration").Value);
            token.TrackingId = user.Claims.FirstOrDefault(c => c.Type == "TrackingId").Value;
            return token;
        }
    }
}
