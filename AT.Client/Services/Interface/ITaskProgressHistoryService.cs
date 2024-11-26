using AT.Share.Model;

namespace AT.Client.Services.Interface
{
    public interface ITaskProgressHistoryService
    {
        Task PostTaskProgressHistoryAsync(TaskProgressHistory taskProgressHistory);
        Task<List<TaskProgressHistory>> GetTaskProgressByIdAsync(int id);
    }
}
