using AT.Share.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProgressService
{
    Task<List<ProgressModel>> GetProgressesAsync();
    Task<List<ProgressModel>> GetProgressByIdAsync(int id);
    Task AddProgressAsync(ProgressModel progress);
    Task UpdateProgressAsync(ProgressModel progress);
    Task DeleteProgressAsync(int id);
}
