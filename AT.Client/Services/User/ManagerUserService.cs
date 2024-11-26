using AT.Client.Services.Interface;
using AT.Share.Model;
using System.Net.Http.Json;
using AT.Client.Services.User;

namespace AT.Client.Services.User
{
    public class ManagerUserService : IManagerUserService
    {
        private readonly HttpClient _httpClient;
        public ManagerUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Staff>> GetAllUserAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Staff>>("api/ManagerUser/all-user");
        }
        public async Task<List<GroupUser>> GetAllGroupUserAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<GroupUser>>("api/ManagerUser/all-groupuser");
        }

        public async Task<Staff> GetStaffByIdAsync()
        {
            return await _httpClient.GetFromJsonAsync<Staff>("api/ManagerUser/all-groupuser-byId");
        }

        public async Task<List<StaffInfo>> GetAllUserInfoAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<StaffInfo>>("api/ManagerUser/all-user-info");
        }


        public async Task<StaffInfo> GetUserInfoByIdAsync()
        {
            return await _httpClient.GetFromJsonAsync<StaffInfo>("api/ManagerUser/all-user-info-byId");
        }

        public async Task UpdateUserInfoAsync(int id, Staff staff)
        {
            await _httpClient.PutAsJsonAsync($"api/ManagerUser/{id}", staff);
        }

        public async Task DeleteUserByIdAsync(string id)
        {
            await _httpClient.DeleteAsync($"api/ManagerUser/{id}");
        }
    }
}
