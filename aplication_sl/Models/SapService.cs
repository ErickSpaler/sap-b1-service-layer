using api_sl.Models;
using aplication_sl.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace api_sl.Services
{
    public class SapService
    {
        private readonly SapSettings _settings;
        private string _sessionId;
        private readonly HttpClient _httpClient;

        public SapService(IOptions<SapSettings> options)
        {
            _settings = options.Value;

            // Ignora certificado autoassinado
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler);
        }

        // Gera ou retorna a sessão existente
        public async Task<string> GetSessionIdAsync()
        {
            if (!string.IsNullOrEmpty(_sessionId))
                return _sessionId;

            var loginDto = new LoginDTO
            {
                CompanyDB = _settings.CompanyDB,
                UserName = _settings.UserName,
                Password = _settings.Password,
                Language = _settings.Language,
                Url = _settings.Url
            };

            var json = JsonSerializer.Serialize(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_settings.Url, content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var session = JsonSerializer.Deserialize<SessionDTO>(result);

            _sessionId = session.SessionId;
            return _sessionId;
        }

        public HttpClient GetHttpClient() => _httpClient;
    }
}
