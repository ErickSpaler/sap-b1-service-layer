using System.Text.Json.Serialization;

namespace api_sl.Models
{
    public class Item
    {
        [JsonPropertyName("ItemCode")]
        public string ItemCode { get; set; }

        [JsonPropertyName("ItemName")]
        public string ItemName { get; set; }
    }
}
