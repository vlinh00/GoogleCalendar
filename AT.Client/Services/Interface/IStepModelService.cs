using AT.Share.Model;

namespace AT.Client.Services.Interface
{
    public interface IStepModelService
    {
        Task<List<StepModel>> GetStepModelByOrderCodeAsync(string orderCode);
        Task PostStepModelAsync(StepModel stepModel);
        Task PostListStepModelAsync(List<StepModel> stepModels);
        Task UpdateStepModelAsync(int id, StepModel stepModel);
    }
}
