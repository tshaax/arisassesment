using Newtonsoft.Json;

namespace Aris.ServerTest.Models
{
    public class KoreError
    {

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string Description { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }
    }
}
