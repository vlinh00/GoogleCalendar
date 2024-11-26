using AT.Share.Model;

namespace AT.Client.Services.Interface
{
    public interface IManagerTaskService
    {
        Task<List<TaskModel>> GetAllTaskAsync();
        Task<List<TaskModel>> GetAllTaskByUserIdAsync();
        Task<TaskModel> GetTaskByIdAsync(int id);
        Task AddTaskAsync(TaskModel taskModel);
        Task UpdateTaskAsync(int id, TaskModel taskModel);
        Task DeleteTaskAsync(int id);
        Task UpdateCalendarEventAsync(string calendarId,TaskModel taskModel);
    }
}
