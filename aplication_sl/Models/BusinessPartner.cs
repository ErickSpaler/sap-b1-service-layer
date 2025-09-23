using System.Text.Json.Serialization;

namespace api_sl.Models
{
    public class BusinessPartner
    {
        [JsonPropertyName("CardCode")]
        public string CardCode { get; set; } // Código único do parceiro

        [JsonPropertyName("CardName")]
        public string CardName { get; set; } // Nome do parceiro

        [JsonPropertyName("CardType")]
        public string CardType { get; set; } // "cCustomer", "cSupplier", "cLid"
    }
}
