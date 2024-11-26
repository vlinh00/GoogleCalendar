using AT.Share.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class ProgressService : IProgressService
{
    private readonly HttpClient _httpClient;

    public ProgressService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProgressModel>> GetProgressesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ProgressModel>>("api/Progress");
    }

    public async Task<List<ProgressModel>> GetProgressByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<List<ProgressModel>>($"api/Progress/{id}");
    }

    public async Task AddProgressAsync(ProgressModel progress)
    {
        await _httpClient.PostAsJsonAsync("api/Progress", progress);
    }

    public async Task UpdateProgressAsync(ProgressModel progress)
    {
        await _httpClient.PutAsJsonAsync($"api/Progress/{progress.Id}", progress);
    }

    public async Task DeleteProgressAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/Progress/{id}");
    }
}
