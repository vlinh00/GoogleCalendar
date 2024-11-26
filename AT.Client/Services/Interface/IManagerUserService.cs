using AT.Share.Model;

namespace AT.Client.Services.Interface
{
    public interface IManagerUserService
    {
        Task<List<Staff>> GetAllUserAsync();
        Task<List<GroupUser>> GetAllGroupUserAsync();
        Task<List<StaffInfo>> GetAllUserInfoAsync();
        Task<Staff> GetStaffByIdAsync();
        Task<StaffInfo> GetUserInfoByIdAsync();
        Task UpdateUserInfoAsync(int id, Staff staff);
        Task DeleteUserByIdAsync(string id);
    }
}
