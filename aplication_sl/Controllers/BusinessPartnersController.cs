using api_sl.Models;
using api_sl.Services;
using aplication_sl.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class BusinessPartnersController : ControllerBase
{
    private readonly SapService _sapService;
    private readonly SapSettings _sapSettings;

    public BusinessPartnersController(SapService sapService, IOptions<SapSettings> sapSettings)
    {
        _sapService = sapService;
        _sapSettings = sapSettings.Value;
    }

    private async Task<HttpClient> GetClientAsync()
    {
        var sessionId = await _sapService.GetSessionIdAsync();
        var client = _sapService.GetHttpClient();

        if (client.DefaultRequestHeaders.Contains("Cookie"))
            client.DefaultRequestHeaders.Remove("Cookie");

        client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={sessionId}");
        return client;
    }

    // GET - Lista os Business Partners (limitado a 100)
    [HttpGet]
    public async Task<IActionResult> GetBusinessPartners()
    {
        var client = await GetClientAsync();
        string url = $"{_sapSettings.BaseUrl}BusinessPartners?$select=CardCode,CardName,CardType&$top=100";

        var response = await client.GetAsync(url);
        var result = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var jsonDoc = JsonDocument.Parse(result);
            var partners = new List<BusinessPartner>();

            foreach (var item in jsonDoc.RootElement.GetProperty("value").EnumerateArray())
            {
                partners.Add(new BusinessPartner
                {
                    CardCode = item.GetProperty("CardCode").GetString(),
                    CardName = item.GetProperty("CardName").GetString(),
                    CardType = item.GetProperty("CardType").GetString()
                });
            }

            return Ok(partners);
        }

        return BadRequest(result);
    }

    // POST - Cria um novo Business Partner
    [HttpPost]
    public async Task<IActionResult> CreateBusinessPartner([FromBody] BusinessPartner bp)
    {
        var client = await GetClientAsync();
        string url = $"{_sapSettings.BaseUrl}BusinessPartners";

        var json = JsonSerializer.Serialize(bp);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(url, content);
        var result = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
            return Ok(result);

        return BadRequest(result);
    }

    // PATCH - Atualiza um Business Partner existente
    [HttpPatch("{cardCode}")]
    public async Task<IActionResult> UpdateBusinessPartner(string cardCode, [FromBody] BusinessPartner updatedBP)
    {
        var client = await GetClientAsync();
        string url = $"{_sapSettings.BaseUrl}BusinessPartners('{cardCode}')";

        var json = JsonSerializer.Serialize(updatedBP);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
        {
            Content = content
        };

        var response = await client.SendAsync(request);
        var result = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
            return Ok(result);

        return BadRequest(result);
    }

    // DELETE - Remove um Business Partner
    [HttpDelete("{cardCode}")]
    public async Task<IActionResult> DeleteBusinessPartner(string cardCode)
    {
        var client = await GetClientAsync();
        string url = $"{_sapSettings.BaseUrl}BusinessPartners('{cardCode}')";

        var response = await client.DeleteAsync(url);
        var result = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
            return Ok($"Parceiro {cardCode} deletado com sucesso.");

        return BadRequest(result);
    }


}
