using Newtonsoft.Json;

namespace Aris.ServerTest.Models
{
    public class KoreLink
    {
        [JsonProperty("href")]
        public string Url { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

    }
}
