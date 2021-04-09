using Newtonsoft.Json;

namespace Aris.ServerTest.Models
{
    public class AttributeModel
    {
        [JsonProperty("lines")]
        public string Lines { get; set; }

        [JsonProperty("free-spins")]
        public bool FreeSpins { get; set; }
    }
}