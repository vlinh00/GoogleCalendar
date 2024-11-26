using AT.Client.Services.Interface;
using AT.Share.Model;
using System.Net.Http.Json;

namespace AT.Client.Services.Progress
{
    public class ProgressUpdateService : IProgressUpdateService
    {
        private readonly HttpClient _httpClient;

        public ProgressUpdateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProgressUpdate>> GetProgressUpdatesByOrderCodeAsync(string orderCode)
        {
            return await _httpClient.GetFromJsonAsync<List<ProgressUpdate>>($"/api/ProgressUpdate/{orderCode}");
        }

        public async Task PostProgressUpdateAsync(ProgressUpdate progressUpdate)
        {
            await _httpClient.PostAsJsonAsync("api/ProgressUpdate",progressUpdate);
        }

        public async Task UpdateProgressAsync(int id, ProgressUpdate progressUpdate)
        {
            await _httpClient.PutAsJsonAsync($"api/ProgressUpdate/{id}", progressUpdate);
        }
    }
}
