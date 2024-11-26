using AT.Client.Services.Interface;
using AT.Share.Model;
using System.Net.Http.Json;

namespace AT.Client.Services.Step
{
    public class StepModelService : IStepModelService
    {
        private readonly HttpClient _httpClient;

        public StepModelService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<StepModel>> GetStepModelByOrderCodeAsync(string orderCode)
        {
            var result = await _httpClient.GetFromJsonAsync<List<StepModel>>($"api/StepModel/{orderCode}");
            if (result == null) return null;
            else
                return result;
        }

        public async Task PostListStepModelAsync(List<StepModel> stepModels)
        {
            await _httpClient.PostAsJsonAsync("api/StepModel/list", stepModels);
        }

        public async Task PostStepModelAsync(StepModel stepModel)
        {
            await _httpClient.PostAsJsonAsync("api/StepModel", stepModel);
        }

        public async Task UpdateStepModelAsync(int id, StepModel stepModel)
        {
            await _httpClient.PutAsJsonAsync($"api/StepModel/{id}", stepModel);
        }
    }
}
