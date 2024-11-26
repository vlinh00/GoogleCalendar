using AT.Share.Model;
namespace AT.Client.Services.Interface
{
    public interface IProgressUpdateService
    {
        Task<List<ProgressUpdate>> GetProgressUpdatesByOrderCodeAsync(string orderCode);
        Task PostProgressUpdateAsync(ProgressUpdate progressUpdate);
        Task UpdateProgressAsync(int id, ProgressUpdate progressUpdate);
    }
}
