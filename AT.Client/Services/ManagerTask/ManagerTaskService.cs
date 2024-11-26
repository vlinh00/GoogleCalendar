using AT.Client.Services.Interface;
using AT.Share.Model;
using System.Net.Http.Json;

namespace AT.Client.Services.ManagerTask
{
    public class ManagerTaskService : IManagerTaskService
    {
        private readonly HttpClient _httpClient;
        public ManagerTaskService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<TaskModel>> GetAllTaskAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TaskModel>>("api/ManagerTask/all-task");
        }
        public async Task<List<TaskModel>> GetAllTaskByUserIdAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TaskModel>>("api/ManagerTask/all-task-by-UserId");
        }
        public async Task<TaskModel> GetTaskByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<TaskModel>($"api/ManagerTask/{id}");
        }
        public async Task AddTaskAsync(TaskModel taskModel)
        {
            await _httpClient.PostAsJsonAsync("api/ManagerTask",taskModel);
        }

        public async Task UpdateTaskAsync(int id, TaskModel taskModel)
        {
            await _httpClient.PutAsJsonAsync($"api/ManagerTask/{id}", taskModel);
        }

        public async Task DeleteTaskAsync(int id)
        {
            await _httpClient.DeleteAsync($"api/ManagerTask/{id}");
        }

        public async Task UpdateCalendarEventAsync(string calendarId , TaskModel taskModel)
        {
            await _httpClient.PutAsJsonAsync($"api/GoogleCalendar/{calendarId}",taskModel);
        }
    }
}
