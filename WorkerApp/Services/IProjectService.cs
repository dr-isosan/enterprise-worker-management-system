using WorkerApp.Model;

namespace WorkerApp.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<Proje>> GetAllProjectsAsync();
        Task<Proje?> GetProjectByIdAsync(int id);
        Task<Proje> CreateProjectAsync(Proje project);
        Task<Proje> UpdateProjectAsync(Proje project);
        Task<bool> DeleteProjectAsync(int id);
        Task<IEnumerable<Proje>> GetOverdueProjectsAsync();
        Task<IEnumerable<Proje>> GetActiveProjectsAsync();
        Task<int> CalculateProjectDelayAsync(int projectId);
        Task<decimal> GetProjectCompletionPercentageAsync(int projectId);
    }
}
