namespace aplication_sl.Models
{
    public class SapSettings
    {
        public string Url { get; set; } // ← usado para /Login
        public string BaseUrl { get; set; } // ← usado para Items e BusinessPartners.
        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Language { get; set; }
    }
}
