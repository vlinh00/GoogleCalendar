using AT.Share.Model;
using System.Net.Http.Json;
using System.Text.Json;

public class SearchAccountService
{
    private readonly HttpClient _httpClient;
    public SearchAccountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<InfoAllAccount> GetOrderAsync(string id)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<InfoAllAccount>($"/api/SearchAccount/{id}");
            return response;
        }
        catch
        (Exception ex)
        {
            return null;
        }

    }
}
