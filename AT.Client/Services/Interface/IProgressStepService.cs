using AT.Share.Model;
namespace AT.Client.Services.Interface
{
    public interface IProgressStepService
    {
        Task<List<ProgressStep>> GetAllProgressStepAsync();
        Task<List<ProgressStep>> GetProgressStepByTaskIdAsync(int  taskId);
        Task PostProgressStepAsync(ProgressStep progressStep);
    }
}
