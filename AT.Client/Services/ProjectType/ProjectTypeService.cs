using AT.Share.Model;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class ProjectTypeService : IProjectTypeService
{
    private readonly HttpClient _httpClient;

    public ProjectTypeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProjectTypeModel>> GetProjectTypesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ProjectTypeModel>>("api/ProjectType");
    }

    public async Task<ProjectTypeModel> GetProjectTypeByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ProjectTypeModel>($"api/ProjectType/{id}");
    }

    public async Task AddProjectTypeAsync(ProjectTypeModel projectType)
    {
        await _httpClient.PostAsJsonAsync("api/ProjectType", projectType);
    }

    public async Task UpdateProjectTypeAsync(ProjectTypeModel projectType)
    {
        await _httpClient.PutAsJsonAsync($"api/ProjectType/{projectType.Id}", projectType);
    }

    public async Task DeleteProjectTypeAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/ProjectType/{id}");
    }
}
