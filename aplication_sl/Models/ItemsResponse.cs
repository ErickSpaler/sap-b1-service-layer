using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace api_sl.Models
{
    public class ItemsResponse
    {
        [JsonPropertyName("@odata.context")]
        public string Context { get; set; }

        [JsonPropertyName("value")]
        public List<Item> Value { get; set; }

        [JsonPropertyName("@odata.nextLink")]
        public string NextLink { get; set; }
    }
}
