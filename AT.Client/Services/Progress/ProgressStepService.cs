using AT.Client.Services.Interface;
using AT.Share.Model;
using System.Net.Http.Json;

namespace AT.Client.Services.Progress
{

    public class ProgressStepService : IProgressStepService
    {
        private readonly HttpClient _httpClient;

        public ProgressStepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProgressStep>> GetAllProgressStepAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ProgressStep>> ("api/ProgressStep");
        }

        public async Task<List<ProgressStep>> GetProgressStepByTaskIdAsync(int taskId)
        {
            return await _httpClient.GetFromJsonAsync<List<ProgressStep>>($"api/ProgressStep/{taskId}");
        }

        public async Task PostProgressStepAsync(ProgressStep progressStep)
        {
            await _httpClient.PostAsJsonAsync("/api/ProgressStep", progressStep);
        }
    }
}
