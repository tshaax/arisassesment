using Newtonsoft.Json;

namespace Aris.ServerTest.Models
{
    public class KoreMedal
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("awarded_date")]
        public string Awarded { get; set; }

    }
}
