using ElRawabi_RealEstate_Backend.Modals;

namespace ElRawabi_RealEstate_Backend.Repositories.Interface
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(int id);
        Task AddProjectAsync(Project project);
        void UpdateProject(Project project);
        void DeleteProject(Project project);
        Task SaveChangesAsync();
        Task<Project?> GetProjectWithBuildingsAsync(int projectId);
        Task<IEnumerable<Project>> GetActiveProjectsAsync();
    }
}
