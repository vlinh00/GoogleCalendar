using AT.Share.Model;

namespace AT.Client.Services.Interface
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllDepartmentAsync();
    }
}
