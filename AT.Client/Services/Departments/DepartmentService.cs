using AT.Client.Services.Interface;
using AT.Share.Model;
using System.Net.Http.Json;
namespace AT.Client.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly HttpClient _httpClient;
        public DepartmentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<Department>> GetAllDepartmentAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Department>>("api/Department/all-department");
        }
    }
}
