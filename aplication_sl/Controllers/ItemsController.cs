using api_sl.Models;
using api_sl.Services;
using aplication_sl.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly SapService _sapService;
    private readonly SapSettings _sapSettings;


    public ItemsController(SapService sapService, IOptions<SapSettings> sapSettings)
    {
        _sapService = sapService;
        _sapSettings = sapSettings.Value;
    }

    // Retorna um HttpClient com a sessão configurada
    private async Task<HttpClient> GetClientAsync()
    {
        var sessionId = await _sapService.GetSessionIdAsync();
        var client = _sapService.GetHttpClient();

        // Remove header anterior caso exista
        if (client.DefaultRequestHeaders.Contains("Cookie"))
            client.DefaultRequestHeaders.Remove("Cookie");

        client.DefaultRequestHeaders.Add("Cookie", $"B1SESSION={sessionId}");
        return client;
    }

    //GET - Lista itens
    [HttpGet]
    public async Task<IActionResult> GetItems()
    {
        var client = await GetClientAsync();
        string url = $"{_sapSettings.BaseUrl}Items?$select=ItemCode,ItemName,ForeignName";

        var response = await client.GetAsync(url);
        var result = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var itemsResponse = JsonSerializer.Deserialize<ItemsResponse>(result);
            return Ok(itemsResponse);
        }
        return BadRequest(result);
    }

    // POST - Cria um novo item
    [HttpPost]
    public async Task<IActionResult> CreateItem([FromBody] Item newItem)
    {
        var client = await GetClientAsync();
        string url = $"{_sapSettings.BaseUrl}Items";

        var json = JsonSerializer.Serialize(newItem);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(url, content);
        var result = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
            return Ok(result);

        return BadRequest(result);
    }

    // PATCH - Atualiza um item existente
    [HttpPatch("{itemCode}")]
    public async Task<IActionResult> UpdateItem(string itemCode, [FromBody] Item updatedItem)
    {
        var client = await GetClientAsync();
        string url = $"{_sapSettings.BaseUrl}Items('{itemCode}')";

        var json = JsonSerializer.Serialize(updatedItem);
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

    // DELETE - Remove um item
    [HttpDelete("{itemCode}")]
    public async Task<IActionResult> DeleteItem(string itemCode)
    {
        var client = await GetClientAsync();
        string url = $"{_sapSettings.BaseUrl}Items('{itemCode}')";

        var response = await client.DeleteAsync(url);
        var result = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
            return Ok("Item deletado com sucesso.");

        return BadRequest(result);
    }
}
