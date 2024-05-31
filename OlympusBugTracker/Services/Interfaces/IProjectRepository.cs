using OlympusBugTracker.Models;

namespace OlympusBugTracker.Services.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project> AddProjectAsync(Project project, int companyId);

        Task<IEnumerable<Project>> GetAllProjectsAsync(int companyId);

        Task<IEnumerable<Project>> GetArchivedProjectsAsync(int companyId);

        Task<Project?> GetProjectByIdAsync(int projectId, int companyId);

        Task UpdateProjectAsync(Project project, int companyId);

        Task ArchiveProjectAsync(int projectId, int companyId);

        Task RestoreProjectAsync(int projectId, int companyId);

    }
}
