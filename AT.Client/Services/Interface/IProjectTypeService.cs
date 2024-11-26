using AT.Share.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProjectTypeService
{
    Task<List<ProjectTypeModel>> GetProjectTypesAsync();
    Task<ProjectTypeModel> GetProjectTypeByIdAsync(int id);
    Task AddProjectTypeAsync(ProjectTypeModel projectType);
    Task UpdateProjectTypeAsync(ProjectTypeModel projectType);
    Task DeleteProjectTypeAsync(int id);
}
