using Newtonsoft.Json;
using System.Collections.Generic;

namespace Aris.ServerTest.Models
{
    public class KoreAuthResponse
    {
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("tracking_id")]
        public string TrackingId { get; set; }

        [JsonProperty("tokens")]
        public IEnumerable<KoreAuthToken> Tokens { get; set;}
    }
}
